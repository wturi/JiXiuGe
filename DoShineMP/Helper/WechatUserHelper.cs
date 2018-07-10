﻿using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Helper
{
    public class WechatUserHelper
    {

        public WechatUser Regiet(string realName, string phoneNumber, string openid)
        {
            var db = new ModelContext();
            //var temn = db.TerminalSet.FirstOrDefault(item => item.Name == System.Configuration.ConfigurationManager.AppSettings["termailname"]);
            var usr = WechatHelper.CheckOpenid(openid);


            UserInfo ui = new UserInfo
            {
                Name = realName,
                PhoneNumber = phoneNumber,
                //LastLoginTerminalId = temn.TerminalId,
                LastLoginTime = DateTime.Now,
                CreateDate = DateTime.Now,
            };

            //db.ActiveLogSet.Add(new ActiveLog
            //{
            //    CreateDate = DateTime.Now,
            //    OptionContent = "Regist in Doshine wechat service",
            //    TerminalId = temn.TerminalId,
            //    UserId = ui.UserInfoId,
            //});

            db.UserInfo.Add(ui);
            //usr.UserInfoId = ui.UserInfoId;

            db.SaveChanges();
            db.WechatUserSet.Find(usr.WechatUserId).UserInfoId = ui.UserInfoId;
            db.SaveChanges();

            LogHelper.AddLog("Regist in Doshine wechat service", "", openid);
            return usr;
        }


        public UserInfo Regiet(string phonenumber, string realname)
        {
            try
            {
                var db = new ModelContext();
                UserInfo ui = new UserInfo
                {
                    Name = realname,
                    PhoneNumber = phonenumber,
                    //LastLoginTerminalId = temn.TerminalId,
                    LastLoginTime = DateTime.Now,
                    CreateDate = DateTime.Now,
                };
                db.UserInfo.Add(ui);
                db.SaveChanges();
                return ui;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public UserInfo FindUseInfoByPhone(string phone)
        {
            try
            {
                var db = new ModelContext();
                var ui = db.UserInfo.FirstOrDefault(item => item.PhoneNumber == phone);
                if (ui != null)
                    return ui;
                else
                    return Regiet(phone, "");
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public WechatUser EditUserInfo(string openid, string realName, string phoneNumber, string address)
        {
            var db = new ModelContext();
            var usr = WechatHelper.CheckOpenid(openid);
            usr = WechatHelper.CheckUser(usr);
            if (usr.UserInfoId == null || usr.UserInfoId == 0 || usr.UserInfo == null)
            {
                return null;
            }
            var ui = db.UserInfo.Find(usr.UserInfoId);

            ui.Name = realName;
            ui.PhoneNumber = phoneNumber;
            ui.Address = address;
            db.SaveChanges();

            LogHelper.AddLog("Edit infomation", "", openid);
            return usr;
        }

        /// <summary>
        /// 获取用户信息,返回值的UserInfo字段为null则表示用户未注册
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        public WechatUser GetUserInfo(string openid)
        {
            var wuser = WechatHelper.CheckOpenid(openid);
            var user = WechatHelper.CheckUser(wuser);
            if (user == null || user.UserInfoId == null)
            {
                return wuser;
            }
            return user;
        }
    }
}
