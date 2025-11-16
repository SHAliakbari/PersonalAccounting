# Personal Accounting: AI-Powered Expense & Receipt Manager

*An intelligent personal expense tracker with AI-powered receipt processing, detailed transaction management, and item-level expense sharing for a complete financial overview.*

PersonalAccounting is a C# Blazor Server application built with .NET 8 and SQLite. It's designed for meticulous tracking of personal and shared finances, leveraging modern technologies like Azure AI to automate and simplify expense management.

## Key Features

*   **Three-Sided Transaction Model:** Track complex transactions involving a sender, an exchange (or intermediary), and a receiver for a granular view of your financial activities.
*   **Advanced Receipt Management:**
    *   **AI-Powered OCR:** Uses **Azure AI Document Intelligence** to automatically scan and extract detailed information from uploaded receipts, minimizing manual data entry.
    *   **Automatic Categorization:** Intelligently assigns categories to receipt items based on historical data, learning from your habits.
    *   **Item-Level & Full Receipt Sharing:** Easily split costs by sharing individual items from a receipt or the entire bill with other users.
    *   **Image Processing:** Automatically processes receipt images for optimal OCR results and generates thumbnails for quick reference.
*   **Comprehensive User Management:**
    *   Secure, role-based user accounts (Admin and standard users) powered by ASP.NET Core Identity.
    *   Admin dashboard for managing all users and system settings.
    *   User profiles with financial details like account/card numbers for easier tracking.
*   **Interactive Telegram Bot:**
    *   Receive on-demand financial reports, statements, and notifications directly in Telegram.
    *   Administrative commands for system maintenance, such as database backup and restore.
*   **Multi-Currency Support:** Handle transactions across different currencies with clear tracking of conversion rates.
*   **Secure Cloud Storage:** All attachments and receipt images are securely stored in **Azure Storage**.
*   **Reporting & Statements:** Generate detailed HTML statements for both transfer requests and receipt expenses.
*   **Local-First Database:** Uses a local SQLite database for data privacy and ease of setup.
*   **Modern Web UI:** A responsive and interactive Blazor Server interface for managing your finances.

## Use Cases

*   **Automated Expense Splitting:** Perfect for roommates, families, or group trips. Just upload a receipt, and the app digitizes it, allowing you to split costs item by item.
*   **Detailed Financial Record-Keeping:** Maintain meticulous, auditable records for taxes or personal budgeting with a clear history of all transactions.
*   **Tracking Remittances:** Monitor international money transfers involving multiple intermediaries, ensuring you know exactly where your money is and what you've paid in fees.

## Technology Stack

*   C#
*   .NET 8
*   Blazor Server
*   Entity Framework Core
*   SQLite
*   Azure AI Document Intelligence (for OCR)
*   Azure Storage (for file attachments)
*   Telegram Bot API

## Getting Started

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/SHAliakbari/PersonalAccounting.git
    ```

2.  **Navigate to the project directory:**
    ```bash
    cd PersonalAccounting
    ```

3.  **Configure Environment Variables:**
    Update `launchSettings.json` or your environment provider with the required secrets. See the "Environment Variables" section below.

4.  **Restore NuGet packages:**
    ```bash
    dotnet restore
    ```

5.  **Run the project:**
    ```bash
    dotnet run --project PersonalAccounting.BlazorApp
    ```
    Or if you are using Docker:
    ```bash
    docker-compose up -d
    ```
    The application will be available at `https://localhost:7103`.

For database migrations, follow the [readme in PersonalAccounting.Domain](PersonalAccounting.Domain/readme.md).

### Environment Variables

```json
"environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development",

    "Authentication_Google_ClientId": "your_google_client_id",
    "Authentication_Google_ClientSecret": "your_google_client_secret",

    "Authentication_Telegram_BotToken": "your_telegram_bot_token",
    "Authentication_Telegram_BotWebhookUrl": "your_public_webhook_url_if_not_in_debug",

    "EmailConfig_SMTPServer": "smtp.server.com",
    "EmailConfig_SMTPPort": "587",
    "EmailConfig_Sender": "Your Name",
    "EmailConfig_UserName": "your_email_username",
    "EmailConfig_Password": "your_email_password",

    "Authentication_MasterEmail": "your_admin_account@server.com",

    "AZURE_STORAGE_CONNECTION_STRING": "your_azure_storage_connection_string",
    "AZURE_DOCUMENTAI_ENDPOINT": "your_azure_document_ai_endpoint",
    "AZURE_DOCUMENTAI_APIKEY": "your_azure_document_ai_api_key"
}
```

## Contributing

Contributions are welcome! Please fork the repository, create a feature branch, and submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).

## Future Development

*   Improved reporting and data visualization dashboards.
*   Enhanced documentation and user guides.

---

## Detailed Feature Previews

### AI-Powered Receipt Processing
PersonalAccounting transforms receipt management. Simply upload a photo of your receipt, and the system uses Azure AI to read and populate all the details—merchant name, date, time, and every single line item with its price. From there, you can easily share items with other users.

![Receipt Details](httpss://user-images.githubusercontent.com/5955502/287002072-93481132-1c5b-4699-855c-7029458f8335.png)

### Three-Sided Transaction Model
The application provides a unique, comprehensive view of your finances with its three-sided transaction model. Unlike traditional software, it incorporates an "exchange" or intermediary party, allowing you to meticulously track complex financial flows like international remittances.

![Transfer Requests](https://github.com/SHAliakbari/PersonalAccounting/blob/master/transfer_requests.png?raw=true)

## Contact

Email: shaliakbari@gmail.com