using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using PersonalAccounting.BlazorApp.Components.Account;
using PersonalAccounting.BlazorApp.Components.Receipt_Component.Services;
using PersonalAccounting.Domain.Data;
using PersonalAccounting.Domain.Services;
using PersonalAccounting.Domain.Services.OCR;
using System.IO;
using System.Text.Json;
using Microsoft.JSInterop;
using Microsoft.EntityFrameworkCore;

namespace PersonalAccounting.BlazorApp.Components.Receipt_Component.Pages;

public partial class AddEdit : ComponentBase
{
    [Inject] private ReceiptService Service { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
    [Inject] private ILogger<AddEdit> Logger { get; set; } = default!;
    [Inject] private IJSRuntime _js { get; set; } = default!;
    [Inject] private BlobService BlobService { get; set; } = default!;
    [Inject] private FormRecognizerService FormRecognizerService { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    [Inject] private IdentityUserAccessor UserAccessor { get; set; } = default!;

    private Receipt master { get; set; } = default!;
    private List<ApplicationUser> users = new();
    private bool showConfirmationClearFormReprocess = false;

    private AddEditShares shareModalAll = default!;

    private ReceiptItem? selectedDetail = null;

    private bool isProcessing = false;

    private bool isReceiptImageUploaded = false;

    private bool isNew { get; set; }

    string uploadStatus { get; set; } = string.Empty;

    private string errorMessage = string.Empty;

    [Parameter]
    public string? id { get; set; }

    ApplicationUser user = default!;
    string name = string.Empty;
    bool isAdmin = false;

    private bool isSaving = false;

    protected override async Task OnInitializedAsync()
    {
        user = await UserAccessor.GetRequiredUserAsync(HttpContextAccessor.HttpContext);
        name = user.UserName ?? string.Empty;
        isAdmin = await UserManager.IsInRoleAsync(user, "admin");

        users = await UserManager.Users.ToListAsync();

        isNew = string.IsNullOrEmpty(id);
        if (isNew)
        {
            master = new Receipt();
        }
        else
        {
            await LoadMaster(int.Parse(id!));
        }
    }

    private void OpenShareModalForAll()
    {
        selectedDetail = new ReceiptItem();
    }

    private async Task ShareModalForAllSave()
    {
        if (selectedDetail == null) return;

        foreach (var row in master.Items)
        {
            row.Shares.Clear();
            foreach (var share in selectedDetail.Shares)
            {
                row.Shares.Add(new ReceiptItemShare
                {
                    ReceiptItem = row,
                    Share = share.Share,
                    UserName = share.UserName,
                    UserFullName = share.UserFullName,
                    UserId = share.UserId
                });
            }
        }
    }

    private async Task OpenFile(Receipt receipt)
    {
        var content = await BlobService.Download(receipt.ImageFileName);

        using var stream = new MemoryStream(content);
        stream.Position = 0;
        using var streamRef = new DotNetStreamReference(stream);
        await _js.InvokeVoidAsync("viewFileFromStream", receipt.ImageFileName, streamRef, ImageProcessingHelper.GetMimeTypeForFileExtension(receipt.ImageFileName));
    }

    private void ShowConfirmationClearFormReprocess()
    {
        showConfirmationClearFormReprocess = true;
    }

    private void CancelConfirmationClearFormReprocess()
    {
        showConfirmationClearFormReprocess = false;
    }

    private async Task ConfirmActionClearFormReprocess()
    {
        showConfirmationClearFormReprocess = false;
        await PreProcessImageFile();
    }

    private async Task PreProcessImageFile()
    {
        if (string.IsNullOrEmpty(master.ImageFileName)) return;

        uploadStatus = "Processing";
        isProcessing = true;

        try
        {
            var file = await BlobService.Download(master.ImageFileName);

            using var stream = new MemoryStream(file);
            await FormRecognizerService.ExtractReceiptInfo(stream, master);
            Service.FillEmptyCategories(master);
            uploadStatus = "Processed. Check the form info";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error processing receipt image");
            uploadStatus = $"Error processing image: {ex.Message}";
        }
        finally
        {
            isProcessing = false;
            isReceiptImageUploaded = false;
        }
    }

    private async Task LoadFiles(InputFileChangeEventArgs e)
    {
        try
        {
            long maxFileSize = 1024L * 1024L * 1024L * 2L;
            var file = e.File;

            uploadStatus = "uploading";
            master.Thumbnail = null;
            master.ImageFileName = "";

            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(maxFileSize).CopyToAsync(memoryStream);

            byte[]? thumbnail = null;
            var fileType = Path.GetExtension(file.Name);
            if (fileType.EndsWith("jpg") || fileType.EndsWith("jpeg") || fileType.EndsWith("png"))
            {
                thumbnail = ImageProcessingHelper.GetReducedImage(256, 256, memoryStream);
            }

            var readyToUpload = memoryStream.ToArray();
            var fileName = await BlobService.Upload(file.Name, readyToUpload);

            master.Thumbnail = thumbnail;
            master.ImageFileName = fileName;
            isReceiptImageUploaded = true;

            uploadStatus = "File is ready to Save";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Upload Method");
            uploadStatus = $"Error uploading file: {ex.Message}";
            throw;
        }
    }

    private async Task LoadMaster(int id)
    {
        master = await Service.GetReceiptById(id);
        isReceiptImageUploaded = false;
    }

    public async Task HandleSubmit(EditContext editContext)
    {
        isSaving = true;
        errorMessage = string.Empty;
        try
        {
            if (master.Id == 0)
            {
                await Service.AddReceipt(master);
            }
            else
            {
                await Service.UpdateReceipt(master);
            }
            Navigation.NavigateTo("/Receipts/List");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error saving receipt");
            errorMessage = $"Error saving receipt: {ex.Message}";
        }
        finally
        {
            isSaving = false;
        }
    }

    private void FillPayedByUserInformation(Microsoft.AspNetCore.Components.Web.FocusEventArgs e)
    {
        var foundUser = users.FirstOrDefault(x => x.Email == master.PaidByUserName);
        if (foundUser != null)
        {
            master.PaidByUserFullName = foundUser.FullName;
            master.PaidByUserId = foundUser.Id;
        }
        else
        {
            master.PaidByUserFullName = string.Empty;
            master.PaidByUserId = string.Empty;
        }
    }

    private async Task OnDetailSaved()
    {
        await InvokeAsync(StateHasChanged);
    }
}