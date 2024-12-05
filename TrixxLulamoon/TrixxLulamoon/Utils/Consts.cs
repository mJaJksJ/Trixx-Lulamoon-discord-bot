namespace TrixxLulamoon.Utils
{
    public static class Consts
    {
        public const string ADMIN_SPAWN_MODAL_ROLE_BUTTON_ID = "spawn-modal-role-button";
        public const string ROLE_BUTTON_MODAL_ID = "role-button-modal";
        public const string ROLE_BUTTON_MODAL_CHANNEL_ID = "message-modal-channel";
        public const string ROLE_BUTTON_MODAL_ROLE_ID = "message-modal-role";

        public const string YES_ROLE_BUTTON_ID = "role_button_yes_";
        public const string NO_ROLE_BUTTON_ID = "role_button_no_";
        public static string YesRoleButton(string roleId) => YES_ROLE_BUTTON_ID + roleId;
        public static string NoRoleButton(string roleId) => NO_ROLE_BUTTON_ID + roleId;

        public const string ADMIN_SPAWN_MODAL_MESSAGE_BUTTON_ID = "spawn-modal-message-button";
        public const string MESSAGE_MODAL_ID = "message-modal";
        public const string MESSAGE_MODAL_CHANNEL_ID = "message-modal-channel";
        public const string MESSAGE_MODAL_RESPONSABLE_MESSAGE_ID = "message-modal-responsable-message";
        public const string MESSAGE_MODAL_TEXT_ID = "message-modal-text";
    }
}
