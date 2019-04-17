namespace ERBus.Service
{ 
    public class RoleState
    {
        public string STATE { get; set; }
        public bool VIEW { get; set; }
        public bool ADD { get; set; }
        public bool EDIT { get; set; }
        public bool DELETE { get; set; }
        public bool APPROVAL { get; set; }

        public RoleState()
        {
            STATE = "";
            VIEW = false;
            ADD = false;
            EDIT = false;
            DELETE = false;
            APPROVAL = false;
        }
    }
}
