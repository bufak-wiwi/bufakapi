# bufakapi
API for the BuFaK System

## Cloning the Project
* clone the Project into the folder you like
* Add a few Files (here under gitignore) with keys
#### /bufakapi/BuFaKAPI/BuFaKAPI/appsettings.json
```
{
  "ConnectionStrings": {
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AppSettings": {
    "firebaseApiKey": "<here stands your firebaseAPIKey>",
    "jwtkey": "<here stands your jwtkey-seed>",
    "currentConferenceId": <the id of the current conference>
  },
  "AllowedHosts": "*"
} 
```
#### /bufakapi/BuFaKAPI/BuFaKApi/KeyManager.cs
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuFaKAPI
{
    public class KeyManager
    {
        public static string getDatabaseString()
        {
            return "server=localhost;port=<mysql_port>;database=<your_mysql_database>;user=<your_mysql_username>;password=<your_mysql_password>";
        }

        public static string getDatabaseDevString()
        {
            return "server=localhost;port=<mysql_port>;database=<your_mysql_database>;user=<your_mysql_username>;password=<your_mysql_password>";
        }
    }
}
```
#### /bufakapi/BuFaKAPI/BuFaKAPI/Services/TelegramBot.cs
```
namespace BuFaKAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Telegram.Bot;

    public class TelegramBot
    {
        private readonly TelegramBotClient botClient;

        public TelegramBot()
        {
            this.botClient = new TelegramBotClient("<your_bot_key>");
        }

        public async void SendTextMessage(string message)
        {
            await this.botClient.SendMessageAsync(chatId: "<your_chat_id>", text: message);
        }
    }
}
```
#### /bufakapi/BuFaKAPI/BuFaKAPI/Services/bufak-wiso-e960b1a417d5.json
```
this file contains your Firebase Admin SDK Keyfile
```

## Deploying with Release configuration
* edit the kestrel.service file under /etc/systemd/system/kestrel.service
  * uncomment the line with Release and comment the Development line
  * save the file
* edit the api-build script under /usr/local/bin
  * uncomment the line with dotnet publish Release
  * comment the line with dotnet publish Debug
  * save the file
* write `systemctl daemon-reload`
* then start the script `api-build`
## Deploying with Development configuration
* edit the kestrel.service file under /etc/systemd/system/kestrel.service
  * comment the line with Release and uncomment the Development line
  * save the file
* edit the api-build script under /usr/local/bin
  * comment the line with dotnet publish Release
  * uncomment the line with dotnet publish Debug
  * save the file
* write `systemctl daemon-reload`
* then start the script `api-build`
