using MvvmHelpers;

namespace SafeAuthenticationTestApp.Model
{
    public class ContainerPermissionsModel : ObservableObject
    {
        public string ContName { get; private set; }

        private bool isRequested;

        public bool IsRequested
        {
            get { return isRequested; }
            set { isRequested = value; OnPropertyChanged(); }
        }

        public PermissionSetModel Access { get; set; }

        public ContainerPermissionsModel(string contName)
        {
            ContName = contName;
            Access = new PermissionSetModel();
        }
    }

    public class PermissionSetModel
    {
        public bool Read { get; set; }
        public bool Insert { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool ManagePermissions { get; set; }
    }
}
