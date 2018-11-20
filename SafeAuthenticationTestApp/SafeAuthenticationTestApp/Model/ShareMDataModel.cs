namespace SafeAuthenticationTestApp.Model
{
    public class ShareMDataModel
    {
        public ulong TypeTag { get; private set; }
        public byte[] Name { get; private set; }
        public PermissionSetModel Access { get; set; }

        public ShareMDataModel(ulong typeTag, byte[] name)
        {
            TypeTag = typeTag;
            Name = name;
            Access = new PermissionSetModel();
        }
    }
}
