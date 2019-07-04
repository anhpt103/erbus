namespace ERBus.Service
{ 
    public class RoleState
    {
        public string STATE { get; set; }
        public bool XEM { get; set; }
        public bool THEM { get; set; }
        public bool SUA { get; set; }
        public bool XOA { get; set; }
        public bool DUYET { get; set; }
        public bool GIAMUA { get; set; }
        public bool GIABAN { get; set; }
        public bool GIAVON { get; set; }
        public bool TYLELAI { get; set; }
        public bool BANCHIETKHAU { get; set; }
        public bool BANBUON { get; set; }
        public bool BANTRALAI { get; set; }

        public RoleState()
        {
            STATE = "";
            XEM = false;
            THEM = false;
            SUA = false;
            XOA = false;
            DUYET = false;
            GIAMUA = false;
            GIABAN = false;
            GIAVON = false;
            TYLELAI = false;
            BANCHIETKHAU = false;
            BANBUON = false;
            BANTRALAI = false;
        }
    }
}
