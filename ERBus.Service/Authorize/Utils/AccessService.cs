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
        RoleState GetRoleStateByMaChucNang(string unitCode, string username, string machucnang);
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
        public RoleState GetRoleStateByMaChucNang(string unitCode, string username, string machucnang)
        {
            RoleState roleState = new RoleState();
            roleState.STATE = machucnang;
            if (username == "admin")
            {
                roleState = new RoleState()
                {
                    STATE = machucnang,
                    XEM = true,
                    THEM = true,
                    SUA = true,
                    XOA = true,
                    DUYET = true,
                    GIAMUA = true,
                    GIABAN = true,
                    GIAVON = true,
                    TYLELAI = true,
                    BANCHIETKHAU = true,
                    BANBUON = true,
                    BANTRALAI = true
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
                            @"SELECT NHOMQUYEN_MENU.XEM,NHOMQUYEN_MENU.THEM,NHOMQUYEN_MENU.SUA,NHOMQUYEN_MENU.XOA,NHOMQUYEN_MENU.DUYET,NHOMQUYEN_MENU.GIAMUA,NHOMQUYEN_MENU.GIABAN,NHOMQUYEN_MENU.GIAVON,NHOMQUYEN_MENU.TYLELAI,NHOMQUYEN_MENU.BANCHIETKHAU,NHOMQUYEN_MENU.BANBUON,NHOMQUYEN_MENU.BANTRALAI FROM NHOMQUYEN_MENU WHERE UNITCODE LIKE '" + unitCode + "%' AND MA_MENU='" + machucnang +
                            "' AND MANHOMQUYEN IN (SELECT MANHOMQUYEN FROM NGUOIDUNG_NHOMQUYEN WHERE UNITCODE LIKE '" + unitCode + "%' AND USERNAME='" +
                            username + "') UNION SELECT NGUOIDUNG_MENU.XEM,NGUOIDUNG_MENU.THEM,NGUOIDUNG_MENU.SUA,NGUOIDUNG_MENU.XOA,NGUOIDUNG_MENU.DUYET,NGUOIDUNG_MENU.GIAMUA,NGUOIDUNG_MENU.GIABAN,NGUOIDUNG_MENU.GIAVON,NGUOIDUNG_MENU.TYLELAI,NGUOIDUNG_MENU.BANCHIETKHAU,NGUOIDUNG_MENU.BANBUON,NGUOIDUNG_MENU.BANTRALAI " +
                            "FROM NGUOIDUNG_MENU WHERE NGUOIDUNG_MENU.UNITCODE LIKE '" + unitCode + "%' AND NGUOIDUNG_MENU.MA_MENU='" + machucnang + "' AND NGUOIDUNG_MENU.USERNAME='" + username + "'";
                        using (OracleDataReader oracleDataReader = command.ExecuteReader())
                        {
                            if (!oracleDataReader.HasRows)
                            {
                                roleState = new RoleState()
                                {
                                    STATE = string.Empty,
                                    XEM = false,
                                    THEM = false,
                                    SUA = false,
                                    XOA = false,
                                    DUYET = false,
                                    GIAMUA = false,
                                    GIABAN = false,
                                    GIAVON = false,
                                    TYLELAI = false,
                                    BANCHIETKHAU = false,
                                    BANBUON = false,
                                    BANTRALAI = false
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
                                        roleState.XEM = true;
                                    }
                                    int objThem = Int32.Parse(oracleDataReader["THEM"].ToString());
                                    if (objThem == 1)
                                    {
                                        roleState.THEM = true;
                                    }
                                    int objSua = Int32.Parse(oracleDataReader["SUA"].ToString());
                                    if (objSua == 1)
                                    {
                                        roleState.SUA = true;
                                    }
                                    int objXoa = Int32.Parse(oracleDataReader["XOA"].ToString());
                                    if (objXoa == 1)
                                    {
                                        roleState.XOA = true;
                                    }
                                    int objDuyet = Int32.Parse(oracleDataReader["DUYET"].ToString());
                                    if (objDuyet == 1)
                                    {
                                        roleState.DUYET = true;
                                    }

                                    int objGIAMUA = Int32.Parse(oracleDataReader["GIAMUA"].ToString());
                                    if (objGIAMUA == 1)
                                    {
                                        roleState.GIAMUA = true;
                                    }

                                    int objGIABAN = Int32.Parse(oracleDataReader["GIABAN"].ToString());
                                    if (objGIABAN == 1)
                                    {
                                        roleState.GIABAN = true;
                                    }

                                    int objGIAVON = Int32.Parse(oracleDataReader["GIAVON"].ToString());
                                    if (objGIAVON == 1)
                                    {
                                        roleState.GIAVON = true;
                                    }

                                    int objTYLELAI = Int32.Parse(oracleDataReader["TYLELAI"].ToString());
                                    if (objTYLELAI == 1)
                                    {
                                        roleState.TYLELAI = true;
                                    }

                                    int objBANCHIETKHAU = Int32.Parse(oracleDataReader["BANCHIETKHAU"].ToString());
                                    if (objBANCHIETKHAU == 1)
                                    {
                                        roleState.BANCHIETKHAU = true;
                                    }

                                    int objBANBUON = Int32.Parse(oracleDataReader["BANBUON"].ToString());
                                    if (objBANBUON == 1)
                                    {
                                        roleState.BANBUON = true;
                                    }

                                    int objBANTRALAI = Int32.Parse(oracleDataReader["BANTRALAI"].ToString());
                                    if (objBANTRALAI == 1)
                                    {
                                        roleState.BANTRALAI = true;
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
