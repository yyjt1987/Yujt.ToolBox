namespace Yujt.ToolBox.EmailRegister.Services
{
    public interface IEmailRegister
    {
        bool IsEmailNameAvailable(string name);

        bool Register(string name, string password, string verifyCode, out string secondImageUrl);

        string GetFirstVerifyCodeImagePath();

        string GetSecondVerifyCodeImagePath();

        bool SecondVerifyPost(string vcode);
    }
}
