using Microsoft.Extensions.Configuration;
using Serilog;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;


namespace Infrastructure.Services
{
    public class VerificationService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _verifyServiceSid;
        
        public VerificationService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _verifyServiceSid = configuration["Twilio:VerifyServiceSid"];
        }
        public bool SendVerificationCode(string phoneNumber, string channel = "sms")  // channel can be "sms" or "call"
        {
            try
            {
                TwilioClient.Init(_accountSid, _authToken);

                var verification = VerificationResource.Create(
                    pathServiceSid: _verifyServiceSid,
                    to: phoneNumber,
                    channel: channel
                );

                return verification.Status == "pending"; // Check if the request was successful
            }
            catch (TwilioException ex)
            {
                // Log the exception for debugging.  Don't expose details to end-users.
                Log.Error($"Twilio Error: {ex.Message}");
                return false; // Indicate failure
            }
            catch (Exception ex) // Catch other potential exceptions
            {
                Log.Error($"General Error: {ex.Message}");
                return false;
            }
        }


        public bool VerifyCode(string phoneNumber, string code)
        {
            try
            {
                TwilioClient.Init(_accountSid, _authToken);

                var verificationCheck = VerificationCheckResource.Create(
                    pathServiceSid: _verifyServiceSid,
                    to: phoneNumber,
                    code: code
                );

                return verificationCheck.Status == "approved"; // Check if verification was successful
            }
            catch (TwilioException ex)
            {
                Log.Error($"Twilio Error: {ex.Message}");
                return false;
            }
            catch (Exception ex) // Catch other potential exceptions
            {
                Log.Error($"General Error: {ex.Message}");
                return false;
            }

        }
    }
}
