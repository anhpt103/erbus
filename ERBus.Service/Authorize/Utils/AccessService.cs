using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq.Expressions;
using ERBus.Service.Service;
using ERBus.Service;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity;

namespace BTS.API.SERVICE.Authorize
{
    public interface IAccessService : IDataInfoService<MENU>
    {
        RoleState GetRoleStateByMaChucNang(string unitCode, string username,string machucnang);
    }
    public class AccessService : DataInfoServiceBase<MENU>, IAccessService
    {
        public AccessService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<MENU, bool>> GetKeyFilter(MENU instance)
        {
            return x => x.MA_MENU == instance.MA_MENU;
        }
        public RoleState GetRoleStateByMaChucNang(string unitCode,string username, string machucnang)
        {
            RoleState roleState = new RoleState();
            roleState.STATE = machucnang;
            if (username == "admin")
            {
                roleState = new RoleState()
                {
                    STATE = machucnang,
                    VIEW = true,
                    APPROVAL = true,
                    DELETE = true,
                    ADD = true,
                    EDIT = true
                };
            }
            else
            {
                using (var connection = new OracleConnection(new ERBusContext().Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            @"SELECT AU_NHOMQUYEN_CHUCNANG.XEM,AU_NHOMQUYEN_CHUCNANG.THEM,AU_NHOMQUYEN_CHUCNANG.SUA,AU_NHOMQUYEN_CHUCNANG.XOA,AU_NHOMQUYEN_CHUCNANG.DUYET,AU_NHOMQUYEN_CHUCNANG.GIAMUA,AU_NHOMQUYEN_CHUCNANG.GIABAN,AU_NHOMQUYEN_CHUCNANG.GIAVON,AU_NHOMQUYEN_CHUCNANG.TYLELAI,AU_NHOMQUYEN_CHUCNANG.BANCHIETKHAU,AU_NHOMQUYEN_CHUCNANG.BANBUON,AU_NHOMQUYEN_CHUCNANG.BANTRALAI FROM AU_NHOMQUYEN_CHUCNANG WHERE UNITCODE='" + unitCode + "' AND MACHUCNANG='" + machucnang +
                            "' AND MANHOMQUYEN IN (SELECT MANHOMQUYEN FROM AU_NGUOIDUNG_NHOMQUYEN WHERE UNITCODE='" + unitCode + "' AND USERNAME='" +
                            username + "') UNION SELECT AU_NGUOIDUNG_QUYEN.XEM,AU_NGUOIDUNG_QUYEN.THEM,AU_NGUOIDUNG_QUYEN.SUA,AU_NGUOIDUNG_QUYEN.XOA,AU_NGUOIDUNG_QUYEN.DUYET,AU_NGUOIDUNG_QUYEN.GIAMUA,AU_NGUOIDUNG_QUYEN.GIABAN,AU_NGUOIDUNG_QUYEN.GIAVON,AU_NGUOIDUNG_QUYEN.TYLELAI,AU_NGUOIDUNG_QUYEN.BANCHIETKHAU,AU_NGUOIDUNG_QUYEN.BANBUON,AU_NGUOIDUNG_QUYEN.BANTRALAI " +
                            "FROM AU_NGUOIDUNG_QUYEN WHERE AU_NGUOIDUNG_QUYEN.UNITCODE='" + unitCode + "' AND AU_NGUOIDUNG_QUYEN.MACHUCNANG='" + machucnang + "' AND AU_NGUOIDUNG_QUYEN.USERNAME='" + username + "'";
                        using (OracleDataReader oracleDataReader = command.ExecuteReader())
                        {
                            if (!oracleDataReader.HasRows)
                            {
                                roleState = new RoleState()
                                {
                                    STATE = string.Empty,
                                    VIEW = false,
                                    APPROVAL = false,
                                    DELETE = false,
                                    ADD = false,
                                    EDIT = false,
                                };
                            }
                            else
                            {
                                roleState.STATE = machucnang;
                                while (oracleDataReader.Read())
                                {
                                    int objXem = Int32.Parse(oracleDataReader["XEM"].ToString());
                                    if (objXem == 1)
                                    {
                                        roleState.VIEW = true;
                                    }
                                    int objThem = Int32.Parse(oracleDataReader["THEM"].ToString());
                                    if (objThem == 1)
                                    {
                                        roleState.ADD = true;
                                    }
                                    int objSua = Int32.Parse(oracleDataReader["SUA"].ToString());
                                    if (objSua == 1)
                                    {
                                        roleState.EDIT = true;
                                    }
                                    int objXoa = Int32.Parse(oracleDataReader["XOA"].ToString());
                                    if (objXoa == 1)
                                    {
                                        roleState.DELETE = true;
                                    }
                                    int objDuyet = Int32.Parse(oracleDataReader["DUYET"].ToString());
                                    if (objDuyet == 1)
                                    {
                                        roleState.APPROVAL = true;
                                    }
                                }
                                MemoryCacheHelper.Add(unitCode + "|" + machucnang + "|" + username, roleState, DateTimeOffset.Now.AddHours(6));
                            }
                        }
                    }
                }
            }
            return roleState;
        }
    }
}
