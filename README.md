# Omnigage .NET SDK ![.NET Core](https://github.com/omnigage/omnigage-sdk-dotnet/workflows/.NET%20Core/badge.svg)

- NuGet package: https://www.nuget.org/packages/Omnigage.SDK/
- Examples: https://github.com/omnigage/omnigage-sdk-examples

## Authorization

### API Token

```csharp
    OmnigageClient.Init("token-key", "token-secret");
```

### JWT Token

```csharp
    OmnigageClient.Init("token");
```

## Samples

1. Call a Single Number

```csharp
    OmnigageClient.Init("token-key", "token-secret");

    var call = new CallResource
    {
        To = "+11115551111"
    };

    await call.Create();
```

2. Call Two Numbers and Connect

```csharp
    OmnigageClient.Init("token-key", "token-secret");

    var call = new CallResource
    {
        From = "+11115550000",
        To = "+11115551111",
        CallerId = new CallerIdResource
        {
            Id = "<insert-caller-id>"
        }
    };

    await call.Create();
```

3. Send a Text Message

```csharp
    OmnigageClient.Init("token-key", "token-secret");

    var message = new TextMessageResource
    {
        Body = "Sample body"
    };

    await message.Create();

    var text = new TextResource
    {
        To = "+11115551111",
        TextMessage = message,
        PhoneNumber = new PhoneNumberResource
        {
            Id = "<insert-phone-number-id>"
        }
    };

    await text.Create();
```

4. Send an Email

```csharp
    OmnigageClient.Init("token-key", "token-secret");

    var message = new EmailMessageResource
    {
        Subject = "Hello",
        Body = "Sample body"
    };

    await message.Create();

    var email = new EmailResource
    {
        To = "demo@omnigage.com",
        EmailMessage = message,
        EmailId = new EmailIdResource
        {
            Id = "<insert-email-id>"
        }
    };

    await email.Create();
```
