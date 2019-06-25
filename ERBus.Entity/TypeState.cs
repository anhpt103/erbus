namespace ERBus.Entity
{
    public enum TypeState
    {
        USED = 10,
        NOTUSED = 0,
        APPROVAL = 10,
        NOTAPPROVAL = 0,
        C, //CREATED
        U, //UPDATE
        ASYNC, //ASYNC
        I, //IMPORT EXCEL
        CLOSE = 10, // ESTABLISHED ROOM
        OPEN = 0, //VACATE ROOM
    }
}
