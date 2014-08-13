using System.Collections.Generic;
using Yujt.ToolBox.EmailRegister.ViewModels;

namespace Yujt.ToolBox.EmailRegister.Services
{
    public interface IEmailPersistentService
    {
        void Save(Email email);

        IList<Email> GetEmaiList();
    }
}
