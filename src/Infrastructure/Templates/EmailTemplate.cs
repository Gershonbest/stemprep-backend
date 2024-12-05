namespace Infrastructure.Templates;
public class EmailTemplate
{
    public static string GetBody(string FirstName, string ApplicationName)
    {
        string data = $"""

            We're thrilled to welcome you to the {ApplicationName} community!

            Get started Your learning started with Our System:

            If you have any questions or need assistance, don't hesitate to reach out to our friendly support team:

            Email us at: Resources@Revent.gov.biz
            
            We're excited to have you on board and can't wait to see what you achieve with {ApplicationName}!

            Best regards,

            The {ApplicationName} Team
            """;

        return data;
    }

    public static string GetSubject(string FirstName, string ApplicationName)
    {
        string data = $"""Welcome to {ApplicationName}, {FirstName}!""";

        return data;
    }

    public static string GetTemplate()
    {
        string htmlTemplate = @"
                                   <!DOCTYPE html>
                                   <html>
                                   <head>
                                       <style>
                                           .header {
                                               background-color: #f0f0f0;
                                               padding: 20px;
                                           }
                                   
                                           .body {
                                               background-color: #fff;
                                               padding: 20px;
                                           }
                                   
                                           .code {
                                               font-weight: bold;
                                               font-size: 18px;
                                           }
                                   
                                           .footer {
                                               background-color: #f0f0f0;
                                               padding: 20px;
                                               text-align: center;
                                           }
                                       </style>
                                   </head>
                                   <body>
                                       <div class=""header"">
                                           <h2>{subject}</h2>
                                       </div>
                                   
                                       <div class=""body"">
                                           {body}
                                       </div>
                                   
                                       <div class=""footer"">
                                           <p>Contact us | About us | Services</p>
                                           <p>&copy; 2024 StemPrep. All rights reserved.</p>
                                       </div>
                                   </body>
                                   </html>
                          ";
        return htmlTemplate;
    }
}