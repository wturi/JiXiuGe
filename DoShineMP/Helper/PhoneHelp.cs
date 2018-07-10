using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoShineMP.Helper
{
    public class PhoneHelp
    {
        public static WechatUserHelper wuser = new WechatUserHelper();
        public static RepairHelper repairHelper = new RepairHelper();
        public static Controllers.IdentifyingCodeController identifyingcode = new Controllers.IdentifyingCodeController();

        public static DataOfLasterRepair FindDataOfLasterRepairByPhone(string phone)
        {

            DataOfLasterRepair dolr = new Helper.DataOfLasterRepair();
            dolr.Village = repairHelper.GetAllVillage().FirstOrDefault(item => item.Name == RecordHelper.GetRecordByPhone(phone).Address);
            var ceshi = repairHelper.GetHistoryRepairByPhone(phone);

            dolr.Repair = ceshi == null ? null : ceshi.ToList();
            return dolr;
        }

        public static List<Models.Repair> FindDataOfRepairHistory(string phone)
        {
            List<Models.Repair> r = new List<Models.Repair>();
            r.AddRange(repairHelper.GetHistoryRepairByPhone(phone, Models.RepairStatus.Apply, 10, 0));
            r.AddRange(repairHelper.GetHistoryRepairByPhone(phone, Models.RepairStatus.Accept, 10, 0));
            r.AddRange(repairHelper.GetHistoryRepairByPhone(phone, Models.RepairStatus.FinishHandle, 10, 0));
            r.AddRange(repairHelper.GetHistoryRepairByPhone(phone, Models.RepairStatus.Finish, 10, 0));
            r.AddRange(repairHelper.GetHistoryRepairByPhone(phone, Models.RepairStatus.Cancel, 10, 0));
            return r;
        }

        public static int Send(string phone)
        {
            try
            {
                int a = identifyingcode.GetIndentifyingCode("openid", phone);
                if (a == 0)
                {
                    return 0;
                }
                else
                {
                    return a;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static int Login(string phone, string send, int sendid)
        {
            try
            {
                if (!IdentifyingCodeHelper.CheckCode(sendid, send, "openid", phone))
                    return 0;
                else if (wuser.FindUseInfoByPhone(phone) != null)
                    return 1;
                else
                    return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static int Login(string phone)
        {
            try
            {
                if (wuser.FindUseInfoByPhone(phone) != null)
                    return 1;
                else
                    return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static DataOfRepair FindDataOfRepair(string phone)
        {
            DataOfRepair dor = new DataOfRepair();

            dor.Village = repairHelper.GetHistoryRepairByPhone(phone).FirstOrDefault();
            dor.Villages = repairHelper.GetAllVillage().ToList();
            dor.OveruseRepair = repairHelper.GetOveruseRepair();
            dor.Record = RecordHelper.GetRecordByPhone(phone);
            dor.HasUnFinishedRepair = repairHelper.HasUnFinishedRepairByPhone(phone);
            return dor;
        }

        public static Models.Repair Repair(string content, string phone, int villageid, string name, int recordid)
        {
            try
            {
                content = content.Replace("\r\n", "<br />");
                return repairHelper.Add(content, phone, villageid, name, recordid);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Models.Repair RepairDetail(int RepairID)
        {
            try
            {
                var r = repairHelper.GetDetail(RepairID);
                return r;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Models.Repair Repairspon(int repairid,string response,int score)
        {
            return repairHelper.Response(repairid, response == null ? "" : response, score);
        }
    }

    public class DataOfLasterRepair
    {
        public Models.UserInfo UserInfo { get; set; }
        public Models.Village Village { get; set; }
        public List<Models.Repair> Repair { get; set; }
    }

    public class DataOfRepair
    {
        public Models.Repair Village { get; set; }
        public Models.Record Record { get; set; }
        public List<Models.Village> Villages { get; set; }
        public int HasUnFinishedRepair { get; set; }
        public Dictionary<string, List<string>> OveruseRepair { get; set; }
    }

}