using MvvmHelpers;

namespace SafeAuthenticationTestApp.Model
{
    public class ContainerPermissionsModel : ObservableObject
    {
        private bool _isRequested;

        private string _contName;

        public string ContName
        {
            get { return _contName; }
            set { _contName = value; OnPropertyChanged(); }
        }

        public bool IsRequested
        {
            get { return _isRequested; }
            set { _isRequested = value; OnPropertyChanged(); }
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
