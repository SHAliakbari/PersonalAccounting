# PersonalAccounting

PersonalAccounting is a C# Blazor web application built with .NET 8 and SQLite, designed to provide comprehensive tracking of personal finances, especially in scenarios involving multiple parties and complex transactions. Unlike traditional accounting systems that focus on two-sided transactions, PersonalAccounting utilizes a three-sided model, incorporating an "exchange" or intermediary, to provide a more granular view of your financial activities.

## Key Features

*   **Three-Sided Transaction Model:** Track transactions involving a sender, an exchange (or intermediary), and a receiver. This allows for detailed tracking of money flow through complex scenarios.
*   **Multi-Currency Support:** Handle transactions involving different currencies for the source and destination, with accurate conversion tracking.
*   **Fee Management:** Account for transaction fees, which can be deducted from either the source or destination currency.
*   **Detailed Audit Trails:** Maintain a complete history of all transactions, providing a clear and auditable record for tax purposes, dispute resolution, or personal record-keeping.
*   **User Management:** Each party involved in a transaction can have a dedicated user account, allowing them to log in and view their specific transaction history.
*   **Telegram Integration:** Receive transaction reports, statements, and other notifications via a dedicated Telegram bot, both for administrators and individual users.
*   **Attachment Support:** Attach multiple files to each transaction, stored securely in Azure Storage, providing additional context and documentation.
*   **SQLite Database:** Uses a local SQLite database for data storage, ensuring privacy and ease of setup.
*   **Blazor Web UI:** Provides a responsive and modern web interface for managing your finances.

## Use Cases

PersonalAccounting is particularly useful for:

*   **Tracking Remittances:** Monitor international money transfers involving multiple intermediaries, ensuring you know exactly where your money is and how much you've paid in fees.
*   **Managing Complex Financial Relationships with Third Parties:** Track transactions involving multiple entities, providing a transparent and auditable record of all financial interactions.
*   **Detailed Financial Record-Keeping:** Maintain meticulous records for legal, tax, or personal reasons, with a clear audit trail of all transactions.

**Example:**

A business may use PersonalAccounting to track payments to suppliers through a payment processor. The business (source), the payment processor (exchange), and the supplier (destination) each have user accounts and can access relevant transaction details. Attachments, such as invoices or contracts, can be associated with each transaction. Both the business and the supplier can receive transaction notifications and statements via Telegram.

## Technology Stack

*   C#
*   Blazor WebAssembly
*   .NET 8
*   SQLite
*   Azure Storage
*   Telegram Bot API

## Getting Started

To get started with PersonalAccounting, follow these steps:

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/SHAliakbari/PersonalAccounting.git
    ```

2.  **Navigate to the project directory:**
    ```bash
    cd PersonalAccounting
    ```

3.  **Restore NuGet packages:**
    ```bash
    dotnet restore
    ```

4.  **Build the project:**
    ```bash
    dotnet build
    ```

5.  **Run the project:**
    ```bash
    dotnet run --project PersonalAccounting.BlazorApp
    ```
    Or if you are using docker : 
    ```bash
    docker-compose up -d
    ```

    This will launch the application in your default web browser. The default port is usually `https://localhost:7103`.

    for new migration follow [readme in PersonalAccounting.Domain](PersonalAccounting.Domain/readme.md)

Note : enviroment variables : 

```bash
"environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "Authentication_Google_ClientId": "your_client_id",
        "Authentication_Google_ClientSecret": "your_client-secret_",

        "Authentication_Telegram_BotToken": "your_telegram_bot",
        "Authentication_Telegram_BotWebhookUrl": "",

        "EmailConfig_SMTPServer": "smtp.server.com",
        "EmailConfig_SMTPPort": "587",
        "EmailConfig_Sender": "your_name_",
        "EmailConfig_UserName": "username",
        "EmailConfig_Password": "password",

        "Authentication_MasterEmail": "master_account_@server.com",

        "AZURE_STORAGE_CONNECTION_STRING": "azure_storage_connection_"

      }
```

## Contributing

Contributions are welcome! If you'd like to contribute to PersonalAccounting, please follow these guidelines:

1.  **Fork the repository:** Create your own fork of the PersonalAccounting repository.

2.  **Create a branch:** Create a new branch for your feature or bug fix:
    ```bash
    git checkout -b feature/your-feature-name
    ```

3.  **Make your changes:** Implement your changes and commit them with clear and concise commit messages.

4.  **Push to your fork:** Push your changes to your forked repository:
    ```bash
    git push origin feature/your-feature-name
    ```

5.  **Submit a pull request:** Open a pull request on the original PersonalAccounting repository, explaining the changes you've made.

Please ensure your code follows the existing coding style and includes appropriate tests.


## License

This project is licensed under the [MIT License](LICENSE).

## Future Development

*   Improved reporting and visualization of financial data.
*   Integration with online banking services (potential future feature).
*   Mobile app support (potential future feature).

## Contact

Email : shaliakbari@gmail.com