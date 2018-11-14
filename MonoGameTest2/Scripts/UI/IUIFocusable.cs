namespace MonoGameTest2.UI
{
    interface IUIFocusable
    {
        bool Focused { get; set; }

        void Focus();
        void Unfocus();
        void FocusUpdate();
    }
}
