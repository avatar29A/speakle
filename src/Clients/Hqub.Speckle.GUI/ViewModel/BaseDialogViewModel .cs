using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.GUI.ViewModel
{
    public class BaseDialogViewModel : BaseViewModel
    {
        #region .ctor

        public BaseDialogViewModel()
        {

        }

        public BaseDialogViewModel(Action closeAction)
        {
            SetCloseAction(closeAction);
        }

        #endregion

        #region Close dialog

        public void SetCloseAction(Action closeAction)
        {
            Close = closeAction;
        }

        //
        //Безопасный метод закрытия диалогового окна
        //
        protected void CloseDialog()
        {
            /*
             * Метод не должен содержать ни какой логики.
             * Если требуется сделать, что то после закрытия требуется заместить метод
             * ClosingCommandExecute
             */
            if (Close != null)
                Close();
        }

        private Action Close { get; set; }

        #endregion
    }
}
