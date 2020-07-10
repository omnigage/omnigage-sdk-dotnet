# Omnigage .NET SDK ![.NET Core](https://github.com/omnigage/omnigage-sdk-dotnet/workflows/.NET%20Core/badge.svg)

Available NuGet: https://www.nuget.org/packages/Omnigage.SDK/

## Example

1. Call a Single Number

```csharp
    OmnigageClient.Init("token-key", "token-secret");

    var call = new CallResource();
    call.To = "+11115551111";

    await call.Create();
```

2. Call Two Numbers and Connect

```csharp
    OmnigageClient.Init("token-key", "token-secret");

    var call = new CallResource();
    call.From = "+11115550000";
    call.To = "+11115551111";
    call.CallerId = new CallerIdResource
    {
        Id = "<insert-caller-id>"
    };

    await call.Create();
```

3. Send a Text Message

```csharp
    OmnigageClient.Init("token-key", "token-secret");

    var textMessage = new TextMessageResource();
    textMessage.Body = "Sample body";

    await textMessage.Create();

    var text = new TextResource();
    text.To = "+11115551111";
    text.TextMessage = textMessage;
    text.PhoneNumber = new PhoneNumberResource
    {
        Id = "<insert-phone-number-id>"
    };

    await text.Create();
```

4. Send an Email

```csharp
    OmnigageClient.Init("token-key", "token-secret");

    var emailMessage = new EmailMessageResource();
    emailMessage.Subject = "Hello";
    emailMessage.Body = "Sample body";

    await emailMessage.Create();

    var email = new EmailResource();
    email.To = "demo@omnigage.com";
    email.EmailMessage = emailMessage;
    email.EmailId = new EmailIdResource
    {
        Id = "<insert-email-id>"
    };

    await email.Create();
```
