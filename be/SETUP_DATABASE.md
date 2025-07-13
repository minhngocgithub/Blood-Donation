# Setup Instructions

## Database Configuration

1. Copy `appsettings.Development.template.json` to `appsettings.Development.json`
2. Update the connection string in `appsettings.Development.json` with your SQL Server details:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=YOUR_SERVER_NAME\\SQLEXPRESS;Initial Catalog=Blood_Donation;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
     }
   }
   ```
3. Replace `YOUR_SERVER_NAME` with your actual SQL Server instance name

## Important Notes

- **Never commit `appsettings.Development.json`** - it contains sensitive database connection information
- The file is already added to `.gitignore` to prevent accidental commits
- Use the template file as a reference for the required configuration structure
