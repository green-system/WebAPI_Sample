using getUserInfo.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Data.Entity;
using System.Net.Http;
using System.Text;
using System;

namespace getUserInfo.Controllers
{
    public class JinjiController : ApiController
    {
        // 引数に社外者フラグがない場合 ⇒ 従業員
        //public IQueryable<UserInfoView> GetEmpInfo(string uid)
        public HttpResponseMessage GetEmpInfo(string uid)
        {
            // システム名（User-Agent）読み取り
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string user_agent = string.Empty;
            if (headers.Contains("User-Agent"))
            {
                user_agent = headers.GetValues("User-Agent").First();
            }

            // User-Agentが管理テーブルのシステム名として登録されているか確認
            using (var context_mainte = new JINJI_MAINTEEntities2())
            {
                // 出力>デバッグにSQLを表示
                context_mainte.Database.Log = x => System.Diagnostics.Debug.WriteLine(x);
                var list = context_mainte.REPLY_PERMISSION
                    .Where(x => x.DELETE_FLAG == false)
                    .GroupJoin(context_mainte.SYSTEM_INFORMATION, a => a.SYSTEM_CODE, b => b.SYSTEM_CODE, (a, b) => new { a, b })
                    .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new
                    {
                        a.a.SYSTEM_CODE,
                        a.a.RETURN_ITEM_CODE,
                        b.SYSTEM_NAME,
                        b.USER_AGENT
                    })
                    .GroupJoin(context_mainte.RETURN_ITEM, ab => ab.RETURN_ITEM_CODE, c => c.RETURN_ITEM_CODE, (ab, c) => new { ab, c })
                    .SelectMany(x => x.c.DefaultIfEmpty(), (ab, c) => new
                    {
                        ab.ab.SYSTEM_CODE,
                        ab.ab.RETURN_ITEM_CODE,
                        ab.ab.SYSTEM_NAME,
                        ab.ab.USER_AGENT,
                        c.RETURN_TABLE_NAME,
                        c.RETURN_ITEM_NAME
                    })
                    .Where(a => a.USER_AGENT == user_agent)
                    .Where(a => a.RETURN_TABLE_NAME == "PERSON_OF_COMPANY")
                    .OrderBy(x => x.RETURN_ITEM_CODE);

                if (list.Any())
                {
                    //foreach (var p in list)
                    //    System.Diagnostics.Debug.WriteLine(
                    //        "System Code: {0}\nReturn Item Code: {1}\nSystem Name: {2}\nUser-Agent: {3}\nReturn Table Name: {4}\nReturn Item Name: {5}",
                    //p.SYSTEM_CODE, p.RETURN_ITEM_CODE, p.SYSTEM_NAME, p.USER_AGENT, p.RETURN_TABLE_NAME, p.RETURN_ITEM_NAME);
                }
                else
                {
                    var resUserAgent = this.Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "システム名が不正です");
                    resUserAgent.Content = new StringContent("システム名が不正です", Encoding.GetEncoding(932), "text/plain");
                    return resUserAgent;
                    //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                // 出力>デバッグに件数を表示
                System.Diagnostics.Debug.WriteLine("応答許可数:{0}件", list.Count());

                //List< UserInfoView > Userlists;
                //Userlists = null;
                using (var context = new JINJIEntities1())
                {
                    //Userlists = (from emp in context.V_PERSON_OF_COMPANY.AsNoTracking()
                    var Userlists = from emp in context.V_PERSON_OF_COMPANY.AsNoTracking()
                                    where emp.STAFF_CODE == uid
                                    //select new UserInfoView
                                    select new
                                    {
                                        STAFF_CODE = emp.STAFF_CODE,
                                        KANJI_NAME = emp.KANJI_NAME,
                                        ROMAN_NAME = emp.ROMAN_NAME,
                                        KANA_NAME = emp.KANA_NAME,
                                        ORIGINAL_NAME_FLAG = emp.ORIGINAL_NAME_FLAG,
                                        KANJI_ORIGINAL_NAME = emp.KANJI_ORIGINAL_NAME,
                                        ROMAN_ORIGINAL_NAME = emp.ROMAN_ORIGINAL_NAME,
                                        KANA_ORIGINAL_NAME = emp.KANA_ORIGINAL_NAME,
                                        MEN_OR_WOMEN = emp.MEN_OR_WOMEN,
                                        STAFF_DIVISION_CODE = emp.STAFF_DIVISION_CODE,
                                        STAFF_DIVISION_NAME = emp.STAFF_DIVISION_NAME,
                                        BELONGING_LATEST_START_DATE = emp.BELONGING_LATEST_START_DATE,
                                        BELONGING_LATEST_CODE = emp.BELONGING_LATEST_CODE,
                                        BBL_BU_CODE = emp.BBL_BU_CODE,
                                        BBL_START_VALIDITY_DATE = emp.BBL_START_VALIDITY_DATE,
                                        BBL_END_VALIDITY_DATE = emp.BBL_END_VALIDITY_DATE,
                                        BBL_BU_NAME_1 = emp.BBL_BU_NAME_1,
                                        BBL_BU_NAME_2 = emp.BBL_BU_NAME_2,
                                        BBL_BU_NAME_3 = emp.BBL_BU_NAME_3,
                                        BBL_BU_INFORMAL_NAME = emp.BBL_BU_INFORMAL_NAME,
                                        BBL_BU_ALTERATION_MARK = emp.BBL_BU_ALTERATION_MARK,
                                        BBL_SECTION_CODE = emp.BBL_SECTION_CODE,
                                        BBL_BU_LOCATION_CODE = emp.BBL_BU_LOCATION_CODE,
                                        BBL_BU_NUMBER = emp.BBL_BU_NUMBER,
                                        BBL_EXECUTIVE_NUMBER = emp.BBL_EXECUTIVE_NUMBER,
                                        BBL_SHITSU_PERSON_SECTION = emp.BBL_SHITSU_PERSON_SECTION,
                                        BBL_ADJUST_SECTION_NUMBER_A = emp.BBL_ADJUST_SECTION_NUMBER_A,
                                        BBL_ADJUST_SECTION_NUMBER_B = emp.BBL_ADJUST_SECTION_NUMBER_B,
                                        BBL_ADJUST_SECTION_NUMBER_C = emp.BBL_ADJUST_SECTION_NUMBER_C,
                                        BBL_ADJUST_SECTION_NUMBER_D = emp.BBL_ADJUST_SECTION_NUMBER_D,
                                        BBL_ASSESSMENT_CODE = emp.BBL_ASSESSMENT_CODE,
                                        BBL_RANK_RISE_CODE = emp.BBL_RANK_RISE_CODE,
                                        BBL_CHANGE_CODE = emp.BBL_CHANGE_CODE,
                                        BBL_TRAINING_CODE = emp.BBL_TRAINING_CODE,
                                        BBL_EMPLOY_OFFICE_CODE = emp.BBL_EMPLOY_OFFICE_CODE,
                                        BBL_NEW_BELONGING_LIMIT_FLAG = emp.BBL_NEW_BELONGING_LIMIT_FLAG,
                                        BBL_PERSONNEL_EXCEPTION = emp.BBL_PERSONNEL_EXCEPTION,
                                        BBL_PERSONNEL_COMPULSION = emp.BBL_PERSONNEL_COMPULSION,
                                        BBL_ENGLISH_NAME = emp.BBL_ENGLISH_NAME,
                                        SBL_SHITSU_CODE = emp.SBL_SHITSU_CODE,
                                        SBL_START_VALIDITY_DATE = emp.SBL_START_VALIDITY_DATE,
                                        SBL_END_VALIDITY_DATE = emp.SBL_END_VALIDITY_DATE,
                                        SBL_BU_CODE = emp.SBL_BU_CODE,
                                        SBL_BU_NAME_1 = emp.SBL_BU_NAME_1,
                                        SBL_BU_NAME_2 = emp.SBL_BU_NAME_2,
                                        SBL_BU_INFORMAL_NAME = emp.SBL_BU_INFORMAL_NAME,
                                        SBL_SHITSU_NAME = emp.SBL_SHITSU_NAME,
                                        SBL_SHITSU_INFORMAL_NAME = emp.SBL_SHITSU_INFORMAL_NAME,
                                        SBL_SHITSU_CHANGE_MARK = emp.SBL_SHITSU_CHANGE_MARK,
                                        SBL_SHITSU_LOCATION_CODE = emp.SBL_SHITSU_LOCATION_CODE,
                                        SBL_SHITSU_KA_NUMBER = emp.SBL_SHITSU_KA_NUMBER,
                                        SBL_SHITSU_KA_DIVISION_CODE = emp.SBL_SHITSU_KA_DIVISION_CODE,
                                        SBL_NEW_BELONGING_LIMIT_FLAG = emp.SBL_NEW_BELONGING_LIMIT_FLAG,
                                        SBL_ENGLISH_NAME = emp.SBL_ENGLISH_NAME,
                                        KBL_KAKARI_CODE = emp.KBL_KAKARI_CODE,
                                        KBL_START_VALIDITY_DATE = emp.KBL_START_VALIDITY_DATE,
                                        KBL_END_VALIDITY_DATE = emp.KBL_END_VALIDITY_DATE,
                                        KBL_BU_CODE = emp.KBL_BU_CODE,
                                        KBL_SHITSU_CODE = emp.KBL_SHITSU_CODE,
                                        KBL_BU_NAME_1 = emp.KBL_BU_NAME_1,
                                        KBL_BU_NAME_2 = emp.KBL_BU_NAME_2,
                                        KBL_BU_INFORMAL_NAME = emp.KBL_BU_INFORMAL_NAME,
                                        KBL_SHITSU_NAME = emp.KBL_SHITSU_NAME,
                                        KBL_SHITSU_INFORMAL_NAME = emp.KBL_SHITSU_INFORMAL_NAME,
                                        KBL_KAKARI_NAME = emp.KBL_KAKARI_NAME,
                                        KBL_KAKARI_INFORMAL_NAME = emp.KBL_KAKARI_INFORMAL_NAME,
                                        KBL_KAKARI_LOCATION_CODE = emp.KBL_KAKARI_LOCATION_CODE,
                                        KBL_KAKARI_NUMBER = emp.KBL_KAKARI_NUMBER,
                                        KBL_KAKARI_DIVISION_CODE = emp.KBL_KAKARI_DIVISION_CODE,
                                        KBL_NEW_BELONGING_LIMIT_FLAG = emp.KBL_NEW_BELONGING_LIMIT_FLAG,
                                        BELONGING_LATEST_BU_NAME = emp.BELONGING_LATEST_BU_NAME,
                                        BELONGING_LATEST_SHITSU_NAME = emp.BELONGING_LATEST_SHITSU_NAME,
                                        BELONGING_LATEST_KAKARI_NAME = emp.BELONGING_LATEST_KAKARI_NAME,
                                        LOCATION_CODE = emp.LOCATION_CODE,
                                        LOCATION_NAME = emp.LOCATION_NAME,
                                        COMPANY_CODE = emp.COMPANY_CODE,
                                        INFORMAL_COMPANY_NAME = emp.INFORMAL_COMPANY_NAME,
                                        JOB_CATEGORY_CODE = emp.JOB_CATEGORY_CODE,
                                        JOB_CATEGORY_NAME = emp.JOB_CATEGORY_NAME,
                                        QUALIFICATION_SYSTEM_CODE = emp.QUALIFICATION_SYSTEM_CODE,
                                        QUALIFICATION_CODE = emp.QUALIFICATION_CODE,
                                        ORIGINAL_BELONGING_FLAG = emp.ORIGINAL_BELONGING_FLAG,
                                        ORIGINAL_BELONGING_START_DATE = emp.ORIGINAL_BELONGING_START_DATE,
                                        ORIGINAL_BELONGING_CODE = emp.ORIGINAL_BELONGING_CODE,
                                        BOB_BU_CODE = emp.BOB_BU_CODE,
                                        BOB_START_VALIDITY_DATE = emp.BOB_START_VALIDITY_DATE,
                                        BOB_END_VALIDITY_DATE = emp.BOB_END_VALIDITY_DATE,
                                        BOB_BU_NAME_1 = emp.BOB_BU_NAME_1,
                                        BOB_BU_NAME_2 = emp.BOB_BU_NAME_2,
                                        BOB_BU_NAME_3 = emp.BOB_BU_NAME_3,
                                        BOB_BU_INFORMAL_NAME = emp.BOB_BU_INFORMAL_NAME,
                                        BOB_BU_ALTERATION_MARK = emp.BOB_BU_ALTERATION_MARK,
                                        BOB_SECTION_CODE = emp.BOB_SECTION_CODE,
                                        BOB_BU_LOCATION_CODE = emp.BOB_BU_LOCATION_CODE,
                                        BOB_BU_NUMBER = emp.BOB_BU_NUMBER,
                                        BOB_EXECUTIVE_NUMBER = emp.BOB_EXECUTIVE_NUMBER,
                                        BOB_SHITSU_PERSON_SECTION = emp.BOB_SHITSU_PERSON_SECTION,
                                        BOB_ADJUST_SECTION_NUMBER_A = emp.BOB_ADJUST_SECTION_NUMBER_A,
                                        BOB_ADJUST_SECTION_NUMBER_B = emp.BOB_ADJUST_SECTION_NUMBER_B,
                                        BOB_ADJUST_SECTION_NUMBER_C = emp.BOB_ADJUST_SECTION_NUMBER_C,
                                        BOB_ADJUST_SECTION_NUMBER_D = emp.BOB_ADJUST_SECTION_NUMBER_D,
                                        BOB_ASSESSMENT_CODE = emp.BOB_ASSESSMENT_CODE,
                                        BOB_RANK_RISE_CODE = emp.BOB_RANK_RISE_CODE,
                                        BOB_CHANGE_CODE = emp.BOB_CHANGE_CODE,
                                        BOB_TRAINING_CODE = emp.BOB_TRAINING_CODE,
                                        BOB_EMPLOY_OFFICE_CODE = emp.BOB_EMPLOY_OFFICE_CODE,
                                        BOB_NEW_BELONGING_LIMIT_FLAG = emp.BOB_NEW_BELONGING_LIMIT_FLAG,
                                        BOB_PERSONNEL_EXCEPTION = emp.BOB_PERSONNEL_EXCEPTION,
                                        BOB_PERSONNEL_COMPULSION = emp.BOB_PERSONNEL_COMPULSION,
                                        BOB_ENGLISH_NAME = emp.BOB_ENGLISH_NAME,
                                        SOB_SHITSU_CODE = emp.SOB_SHITSU_CODE,
                                        SOB_START_VALIDITY_DATE = emp.SOB_START_VALIDITY_DATE,
                                        SOB_END_VALIDITY_DATE = emp.SOB_END_VALIDITY_DATE,
                                        SOB_BU_CODE = emp.SOB_BU_CODE,
                                        SOB_BU_NAME_1 = emp.SOB_BU_NAME_1,
                                        SOB_BU_NAME_2 = emp.SOB_BU_NAME_2,
                                        SOB_BU_INFORMAL_NAME = emp.SOB_BU_INFORMAL_NAME,
                                        SOB_SHITSU_NAME = emp.SOB_SHITSU_NAME,
                                        SOB_SHITSU_INFORMAL_NAME = emp.SOB_SHITSU_INFORMAL_NAME,
                                        SOB_SHITSU_CHANGE_MARK = emp.SOB_SHITSU_CHANGE_MARK,
                                        SOB_SHITSU_LOCATION_CODE = emp.SOB_SHITSU_LOCATION_CODE,
                                        SOB_SHITSU_KA_NUMBER = emp.SOB_SHITSU_KA_NUMBER,
                                        SOB_SHITSU_KA_DIVISION_CODE = emp.SOB_SHITSU_KA_DIVISION_CODE,
                                        SOB_NEW_BELONGING_LIMIT_FLAG = emp.SOB_NEW_BELONGING_LIMIT_FLAG,
                                        SOB_ENGLISH_NAME = emp.SOB_ENGLISH_NAME,
                                        KOB_KAKARI_CODE = emp.KOB_KAKARI_CODE,
                                        KOB_START_VALIDITY_DATE = emp.KOB_START_VALIDITY_DATE,
                                        KOB_END_VALIDITY_DATE = emp.KOB_END_VALIDITY_DATE,
                                        KOB_BU_CODE = emp.KOB_BU_CODE,
                                        KOB_SHITSU_CODE = emp.KOB_SHITSU_CODE,
                                        KOB_BU_NAME_1 = emp.KOB_BU_NAME_1,
                                        KOB_BU_NAME_2 = emp.KOB_BU_NAME_2,
                                        KOB_BU_INFORMAL_NAME = emp.KOB_BU_INFORMAL_NAME,
                                        KOB_SHITSU_NAME = emp.KOB_SHITSU_NAME,
                                        KOB_SHITSU_INFORMAL_NAME = emp.KOB_SHITSU_INFORMAL_NAME,
                                        KOB_KAKARI_NAME = emp.KOB_KAKARI_NAME,
                                        KOB_KAKARI_INFORMAL_NAME = emp.KOB_KAKARI_INFORMAL_NAME,
                                        KOB_KAKARI_LOCATION_CODE = emp.KOB_KAKARI_LOCATION_CODE,
                                        KOB_KAKARI_NUMBER = emp.KOB_KAKARI_NUMBER,
                                        KOB_KAKARI_DIVISION_CODE = emp.KOB_KAKARI_DIVISION_CODE,
                                        KOB_NEW_BELONGING_LIMIT_FLAG = emp.KOB_NEW_BELONGING_LIMIT_FLAG,
                                        ABOUT_WORK_CODE = emp.ABOUT_WORK_CODE,
                                        ABOUT_WORK_NAME = emp.ABOUT_WORK_NAME,
                                        APPROVAL_RANK_CODE = emp.APPROVAL_RANK_CODE,
                                        LAYOFF_FLAG = emp.LAYOFF_FLAG,
                                        NONREGULAR_DIVISION_NAME = emp.NONREGULAR_DIVISION_NAME
                                        //}).ToList();
                                    };

                    if (Userlists.Any() == false)
                    {
                        //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                        var resUserList = this.Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "エラーメッセージ");
                        resUserList.Content = new StringContent("error_code=E0001", Encoding.GetEncoding(932), "text/plain");
                        return resUserList;
                    }

                    // 出力>デバッグにSQLを表示
                    //context.Database.Log = x => System.Diagnostics.Debug.WriteLine(x);
                    System.Diagnostics.Debug.WriteLine("従業員情報:{0}件", Userlists.Count());
                    System.Diagnostics.Debug.WriteLine(user_agent);

                    // 応答許可項目文字列作成
                    string resUserlists = string.Empty;
                    foreach (var v in Userlists)
                    {
                        //System.Diagnostics.Debug.WriteLine($"{v.STAFF_CODE + ":",-15}{v.KANJI_NAME}");
                        foreach (var p in list)
                        {
                            switch (p.RETURN_ITEM_NAME)
                            {
                                case "STAFF_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.STAFF_CODE + ";";
                                    break;
                                case "KANJI_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.KANJI_NAME + ";";
                                    break;
                                case "ROMAN_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ROMAN_NAME + ";";
                                    break;
                                case "KANA_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.KANA_NAME + ";";
                                    break;
                                case "ORIGINAL_NAME_FLAG":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ORIGINAL_NAME_FLAG + ";";
                                    break;
                                case "KANJI_ORIGINAL_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.KANJI_ORIGINAL_NAME + ";";
                                    break;
                                case "ROMAN_ORIGINAL_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ROMAN_ORIGINAL_NAME + ";";
                                    break;
                                case "KANA_ORIGINAL_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.KANA_ORIGINAL_NAME + ";";
                                    break;
                                case "MEN_OR_WOMEN":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.MEN_OR_WOMEN + ";";
                                    break;
                                case "STAFF_DIVISION_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.STAFF_DIVISION_CODE + ";";
                                    break;
                                case "STAFF_DIVISION_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.STAFF_DIVISION_NAME + ";";
                                    break;                                                           
                                case "BELONGING_LATEST_START_DATE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.BELONGING_LATEST_START_DATE + ";";
                                    break;
                                case "BELONGING_LATEST_CODE":
                                    resUserlists = resUserlists
                                        + p.RETURN_ITEM_NAME.ToLower() + "=" + v.BELONGING_LATEST_CODE + ";"
                                        + "bbl_bu_code=" + v.BBL_BU_CODE + ";"
                                        + "bbl_start_validity_date=" + v.BBL_START_VALIDITY_DATE + ";"
                                        + "bbl_end_validity_date=" + v.BBL_END_VALIDITY_DATE + ";"
                                        + "bbl_bu_name_1=" + v.BBL_BU_NAME_1 + ";"
                                        + "bbl_bu_name_2=" + v.BBL_BU_NAME_2 + ";"
                                        + "bbl_bu_name_3=" + v.BBL_BU_NAME_3 + ";"
                                        + "bbl_bu_informal_name=" + v.BBL_BU_INFORMAL_NAME + ";"
                                        + "bbl_bu_alteration_mark=" + v.BBL_BU_ALTERATION_MARK + ";"
                                        + "bbl_section_code=" + v.BBL_SECTION_CODE + ";"
                                        + "bbl_bu_location_code=" + v.BBL_BU_LOCATION_CODE + ";"
                                        + "bbl_bu_number=" + v.BBL_BU_NUMBER + ";"
                                        + "bbl_executive_number=" + v.BBL_EXECUTIVE_NUMBER + ";"
                                        + "bbl_shitsu_person_section=" + v.BBL_SHITSU_PERSON_SECTION + ";"
                                        + "bbl_adjust_section_number_a=" + v.BBL_ADJUST_SECTION_NUMBER_A + ";"
                                        + "bbl_adjust_section_number_b=" + v.BBL_ADJUST_SECTION_NUMBER_B + ";"
                                        + "bbl_adjust_section_number_c=" + v.BBL_ADJUST_SECTION_NUMBER_C + ";"
                                        + "bbl_adjust_section_number_d=" + v.BBL_ADJUST_SECTION_NUMBER_D + ";"
                                        + "bbl_assessment_code=" + v.BBL_ASSESSMENT_CODE + ";"
                                        + "bbl_rank_rise_code=" + v.BBL_RANK_RISE_CODE + ";"
                                        + "bbl_change_code=" + v.BBL_CHANGE_CODE + ";"
                                        + "bbl_training_code=" + v.BBL_TRAINING_CODE + ";"
                                        + "bbl_employ_office_code=" + v.BBL_EMPLOY_OFFICE_CODE + ";"
                                        + "bbl_new_belonging_limit_flag=" + v.BBL_NEW_BELONGING_LIMIT_FLAG + ";"
                                        + "bbl_personnel_exception=" + v.BBL_PERSONNEL_EXCEPTION + ";"
                                        + "bbl_personnel_compulsion=" + v.BBL_PERSONNEL_COMPULSION + ";"
                                        + "bbl_english_name=" + v.BBL_ENGLISH_NAME + ";"
                                        + "sbl_shitsu_code=" + v.SBL_SHITSU_CODE + ";"
                                        + "sbl_start_validity_date=" + v.SBL_START_VALIDITY_DATE + ";"
                                        + "sbl_end_validity_date=" + v.SBL_END_VALIDITY_DATE + ";"
                                        + "sbl_bu_code=" + v.SBL_BU_CODE + ";"
                                        + "sbl_bu_name_1=" + v.SBL_BU_NAME_1 + ";"
                                        + "sbl_bu_name_2=" + v.SBL_BU_NAME_2 + ";"
                                        + "sbl_bu_informal_name=" + v.SBL_BU_INFORMAL_NAME + ";"
                                        + "sbl_shitsu_name=" + v.SBL_SHITSU_NAME + ";"
                                        + "sbl_shitsu_informal_name=" + v.SBL_SHITSU_INFORMAL_NAME + ";"
                                        + "sbl_shitsu_change_mark=" + v.SBL_SHITSU_CHANGE_MARK + ";"
                                        + "sbl_shitsu_location_code=" + v.SBL_SHITSU_LOCATION_CODE + ";"
                                        + "sbl_shitsu_ka_number=" + v.SBL_SHITSU_KA_NUMBER + ";"
                                        + "sbl_shitsu_ka_division_code=" + v.SBL_SHITSU_KA_DIVISION_CODE + ";"
                                        + "sbl_new_belonging_limit_flag=" + v.SBL_NEW_BELONGING_LIMIT_FLAG + ";"
                                        + "sbl_english_name=" + v.SBL_ENGLISH_NAME + ";"
                                        + "kbl_kakari_code=" + v.KBL_KAKARI_CODE + ";"
                                        + "kbl_start_validity_date=" + v.KBL_START_VALIDITY_DATE + ";"
                                        + "kbl_end_validity_date=" + v.KBL_END_VALIDITY_DATE + ";"
                                        + "kbl_bu_code=" + v.KBL_BU_CODE + ";"
                                        + "kbl_shitsu_code=" + v.KBL_SHITSU_CODE + ";"
                                        + "kbl_bu_name_1=" + v.KBL_BU_NAME_1 + ";"
                                        + "kbl_bu_name_2=" + v.KBL_BU_NAME_2 + ";"
                                        + "kbl_bu_informal_name=" + v.KBL_BU_INFORMAL_NAME + ";"
                                        + "kbl_shitsu_name=" + v.KBL_SHITSU_NAME + ";"
                                        + "kbl_shitsu_informal_name=" + v.KBL_SHITSU_INFORMAL_NAME + ";"
                                        + "kbl_kakari_name=" + v.KBL_KAKARI_NAME + ";"
                                        + "kbl_kakari_informal_name=" + v.KBL_KAKARI_INFORMAL_NAME + ";"
                                        + "kbl_kakari_location_code=" + v.KBL_KAKARI_LOCATION_CODE + ";"
                                        + "kbl_kakari_number=" + v.KBL_KAKARI_NUMBER + ";"
                                        + "kbl_kakari_division_code=" + v.KBL_KAKARI_DIVISION_CODE + ";"
                                        + "kbl_new_belonging_limit_flag=" + v.KBL_NEW_BELONGING_LIMIT_FLAG + ";"
                                        ;
                                    break;
                                case "BELONGING_LATEST_BU_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.BELONGING_LATEST_BU_NAME + ";";
                                    break;
                                case "BELONGING_LATEST_SHITSU_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.BELONGING_LATEST_SHITSU_NAME + ";";
                                    break;
                                case "BELONGING_LATEST_KAKARI_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.BELONGING_LATEST_KAKARI_NAME + ";";
                                    break;
                                case "LOCATION_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.LOCATION_CODE + ";";
                                    break;
                                case "LOCATION_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.LOCATION_NAME + ";";
                                    break;
                                case "COMPANY_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.COMPANY_CODE + ";";
                                    break;
                                case "INFORMAL_COMPANY_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.INFORMAL_COMPANY_NAME + ";";
                                    break;
                                case "JOB_CATEGORY_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.JOB_CATEGORY_CODE + ";";
                                    break;
                                case "JOB_CATEGORY_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.JOB_CATEGORY_NAME + ";";
                                    break;
                                case "QUALIFICATION_SYSTEM_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.QUALIFICATION_SYSTEM_CODE + ";";
                                    break;
                                case "QUALIFICATION_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.QUALIFICATION_CODE + ";";
                                    break;
                                case "ORIGINAL_BELONGING_FLAG":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ORIGINAL_BELONGING_FLAG + ";";
                                    break;
                                case "ORIGINAL_BELONGING_START_DATE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ORIGINAL_BELONGING_START_DATE + ";";
                                    break;
                                case "ORIGINAL_BELONGING_CODE":
                                    resUserlists = resUserlists
                                        + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ORIGINAL_BELONGING_CODE + ";"
                                        + "bob_bu_code=" + v.BOB_BU_CODE + ";"
                                        + "bob_start_validity_date=" + v.BOB_START_VALIDITY_DATE + ";"
                                        + "bob_end_validity_date=" + v.BOB_END_VALIDITY_DATE + ";"
                                        + "bob_bu_name_1=" + v.BOB_BU_NAME_1 + ";"
                                        + "bob_bu_name_2=" + v.BOB_BU_NAME_2 + ";"
                                        + "bob_bu_name_3=" + v.BOB_BU_NAME_3 + ";"
                                        + "bob_bu_informal_name=" + v.BOB_BU_INFORMAL_NAME + ";"
                                        + "bob_bu_alteration_mark=" + v.BOB_BU_ALTERATION_MARK + ";"
                                        + "bob_section_code=" + v.BOB_SECTION_CODE + ";"
                                        + "bob_bu_location_code=" + v.BOB_BU_LOCATION_CODE + ";"
                                        + "bob_bu_number=" + v.BOB_BU_NUMBER + ";"
                                        + "bob_executive_number=" + v.BOB_EXECUTIVE_NUMBER + ";"
                                        + "bob_shitsu_person_section=" + v.BOB_SHITSU_PERSON_SECTION + ";"
                                        + "bob_adjust_section_number_a=" + v.BOB_ADJUST_SECTION_NUMBER_A + ";"
                                        + "bob_adjust_section_number_b=" + v.BOB_ADJUST_SECTION_NUMBER_B + ";"
                                        + "bob_adjust_section_number_c=" + v.BOB_ADJUST_SECTION_NUMBER_C + ";"
                                        + "bob_adjust_section_number_d=" + v.BOB_ADJUST_SECTION_NUMBER_D + ";"
                                        + "bob_assessment_code=" + v.BOB_ASSESSMENT_CODE + ";"
                                        + "bob_rank_rise_code=" + v.BOB_RANK_RISE_CODE + ";"
                                        + "bob_change_code=" + v.BOB_CHANGE_CODE + ";"
                                        + "bob_training_code=" + v.BOB_TRAINING_CODE + ";"
                                        + "bob_employ_office_code=" + v.BOB_EMPLOY_OFFICE_CODE + ";"
                                        + "bob_new_belonging_limit_flag=" + v.BOB_NEW_BELONGING_LIMIT_FLAG + ";"
                                        + "bob_personnel_exception=" + v.BOB_PERSONNEL_EXCEPTION + ";"
                                        + "bob_personnel_compulsion=" + v.BOB_PERSONNEL_COMPULSION + ";"
                                        + "bob_english_name=" + v.BOB_ENGLISH_NAME + ";"
                                        + "sob_shitsu_code=" + v.SOB_SHITSU_CODE + ";"
                                        + "sob_start_validity_date=" + v.SOB_START_VALIDITY_DATE + ";"
                                        + "sob_end_validity_date=" + v.SOB_END_VALIDITY_DATE + ";"
                                        + "sob_bu_code=" + v.SOB_BU_CODE + ";"
                                        + "sob_bu_name_1=" + v.SOB_BU_NAME_1 + ";"
                                        + "sob_bu_name_2=" + v.SOB_BU_NAME_2 + ";"
                                        + "sob_bu_informal_name=" + v.SOB_BU_INFORMAL_NAME + ";"
                                        + "sob_shitsu_name=" + v.SOB_SHITSU_NAME + ";"
                                        + "sob_shitsu_informal_name=" + v.SOB_SHITSU_INFORMAL_NAME + ";"
                                        + "sob_shitsu_change_mark=" + v.SOB_SHITSU_CHANGE_MARK + ";"
                                        + "sob_shitsu_location_code=" + v.SOB_SHITSU_LOCATION_CODE + ";"
                                        + "sob_shitsu_ka_number=" + v.SOB_SHITSU_KA_NUMBER + ";"
                                        + "sob_shitsu_ka_division_code=" + v.SOB_SHITSU_KA_DIVISION_CODE + ";"
                                        + "sob_new_belonging_limit_flag=" + v.SOB_NEW_BELONGING_LIMIT_FLAG + ";"
                                        + "sob_english_name=" + v.SOB_ENGLISH_NAME + ";"
                                        + "kob_kakari_code=" + v.KOB_KAKARI_CODE + ";"
                                        + "kob_start_validity_date=" + v.KOB_START_VALIDITY_DATE + ";"
                                        + "kob_end_validity_date=" + v.KOB_END_VALIDITY_DATE + ";"
                                        + "kob_bu_code=" + v.KOB_BU_CODE + ";"
                                        + "kob_shitsu_code=" + v.KOB_SHITSU_CODE + ";"
                                        + "kob_bu_name_1=" + v.KOB_BU_NAME_1 + ";"
                                        + "kob_bu_name_2=" + v.KOB_BU_NAME_2 + ";"
                                        + "kob_bu_informal_name=" + v.KOB_BU_INFORMAL_NAME + ";"
                                        + "kob_shitsu_name=" + v.KOB_SHITSU_NAME + ";"
                                        + "kob_shitsu_informal_name=" + v.KOB_SHITSU_INFORMAL_NAME + ";"
                                        + "kob_kakari_name=" + v.KOB_KAKARI_NAME + ";"
                                        + "kob_kakari_informal_name=" + v.KOB_KAKARI_INFORMAL_NAME + ";"
                                        + "kob_kakari_location_code=" + v.KOB_KAKARI_LOCATION_CODE + ";"
                                        + "kob_kakari_number=" + v.KOB_KAKARI_NUMBER + ";"
                                        + "kob_kakari_division_code=" + v.KOB_KAKARI_DIVISION_CODE + ";"
                                        + "kob_new_belonging_limit_flag=" + v.KOB_NEW_BELONGING_LIMIT_FLAG + ";"
                                        ;
                                    break;
                                case "ABOUT_WORK_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ABOUT_WORK_CODE + ";";
                                    break;
                                case "ABOUT_WORK_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ABOUT_WORK_NAME + ";";
                                    break;
                                case "APPROVAL_RANK_CODE":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.APPROVAL_RANK_CODE + ";";
                                    break;
                                case "LAYOFF_FLAG":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.LAYOFF_FLAG + ";";
                                    break;
                                case "NONREGULAR_DIVISION_NAME":
                                    resUserlists = resUserlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.NONREGULAR_DIVISION_NAME + ";";
                                    break;
                            }
                        }
                        resUserlists = resUserlists.Substring(0, resUserlists.Length - 1) + Environment.NewLine;
                        //resUserlists = resUserlists + "staff_code=" + v.STAFF_CODE + ";" + "kanji_name=" + v.KANJI_NAME + ";" + "belonging_latest_start_date=" + v.BELONGING_LATEST_START_DATE + ";" + "belonging_latest_code=" + v.BELONGING_LATEST_CODE + ";" + "bbl_bu_code=" + v.BBL_BU_CODE + ";" + "bbl_start_validity_date=" + v.BBL_START_VALIDITY_DATE + ";" + "bbl_end_validity_date=" + v.BBL_END_VALIDITY_DATE + ";" + "bbl_bu_name_1=" + v.BBL_BU_NAME_1 + ";" + "bbl_bu_name_2=" + v.BBL_BU_NAME_2 + ";" + "bbl_bu_name_3=" + v.BBL_BU_NAME_3 + ";" + "bbl_bu_informal_name=" + v.BBL_BU_INFORMAL_NAME + ";" + "bbl_bu_alteration_mark=" + v.BBL_BU_ALTERATION_MARK + ";" + "bbl_section_code=" + v.BBL_SECTION_CODE + ";" + "bbl_bu_location_code=" + v.BBL_BU_LOCATION_CODE + ";" + "bbl_bu_number=" + v.BBL_BU_NUMBER + ";" + "bbl_executive_number=" + v.BBL_EXECUTIVE_NUMBER + ";" + "bbl_shitsu_person_section=" + v.BBL_SHITSU_PERSON_SECTION + ";" + "bbl_adjust_section_number_a=" + v.BBL_ADJUST_SECTION_NUMBER_A + ";" + "bbl_adjust_section_number_b=" + v.BBL_ADJUST_SECTION_NUMBER_B + ";" + "bbl_adjust_section_number_c=" + v.BBL_ADJUST_SECTION_NUMBER_C + ";" + "bbl_adjust_section_number_d=" + v.BBL_ADJUST_SECTION_NUMBER_D + ";" + "bbl_assessment_code=" + v.BBL_ASSESSMENT_CODE + ";" + "bbl_rank_rise_code=" + v.BBL_RANK_RISE_CODE + ";" + "bbl_change_code=" + v.BBL_CHANGE_CODE + ";" + "bbl_training_code=" + v.BBL_TRAINING_CODE + ";" + "bbl_employ_office_code=" + v.BBL_EMPLOY_OFFICE_CODE + ";" + "bbl_new_belonging_limit_flag=" + v.BBL_NEW_BELONGING_LIMIT_FLAG + ";" + "bbl_personnel_exception=" + v.BBL_PERSONNEL_EXCEPTION + ";" + "bbl_personnel_compulsion=" + v.BBL_PERSONNEL_COMPULSION + ";" + "bbl_english_name=" + v.BBL_ENGLISH_NAME + ";" + "sbl_shitsu_code=" + v.SBL_SHITSU_CODE + ";" + "sbl_start_validity_date=" + v.SBL_START_VALIDITY_DATE + ";" + "sbl_end_validity_date=" + v.SBL_END_VALIDITY_DATE + ";" + "sbl_bu_code=" + v.SBL_BU_CODE + ";" + "sbl_bu_name_1=" + v.SBL_BU_NAME_1 + ";" + "sbl_bu_name_2=" + v.SBL_BU_NAME_2 + ";" + "sbl_bu_informal_name=" + v.SBL_BU_INFORMAL_NAME + ";" + "sbl_shitsu_name=" + v.SBL_SHITSU_NAME + ";" + "sbl_shitsu_informal_name=" + v.SBL_SHITSU_INFORMAL_NAME + ";" + "sbl_shitsu_change_mark=" + v.SBL_SHITSU_CHANGE_MARK + ";" + "sbl_shitsu_location_code=" + v.SBL_SHITSU_LOCATION_CODE + ";" + "sbl_shitsu_ka_number=" + v.SBL_SHITSU_KA_NUMBER + ";" + "sbl_shitsu_ka_division_code=" + v.SBL_SHITSU_KA_DIVISION_CODE + ";" + "sbl_new_belonging_limit_flag=" + v.SBL_NEW_BELONGING_LIMIT_FLAG + ";" + "sbl_english_name=" + v.SBL_ENGLISH_NAME + ";" + "kbl_kakari_code=" + v.KBL_KAKARI_CODE + ";" + "kbl_start_validity_date=" + v.KBL_START_VALIDITY_DATE + ";" + "kbl_end_validity_date=" + v.KBL_END_VALIDITY_DATE + ";" + "kbl_bu_code=" + v.KBL_BU_CODE + ";" + "kbl_shitsu_code=" + v.KBL_SHITSU_CODE + ";" + "kbl_bu_name_1=" + v.KBL_BU_NAME_1 + ";" + "kbl_bu_name_2=" + v.KBL_BU_NAME_2 + ";" + "kbl_bu_informal_name=" + v.KBL_BU_INFORMAL_NAME + ";" + "kbl_shitsu_name=" + v.KBL_SHITSU_NAME + ";" + "kbl_shitsu_informal_name=" + v.KBL_SHITSU_INFORMAL_NAME + ";" + "kbl_kakari_name=" + v.KBL_KAKARI_NAME + ";" + "kbl_kakari_informal_name=" + v.KBL_KAKARI_INFORMAL_NAME + ";" + "kbl_kakari_location_code=" + v.KBL_KAKARI_LOCATION_CODE + ";" + "kbl_kakari_number=" + v.KBL_KAKARI_NUMBER + ";" + "kbl_kakari_division_code=" + v.KBL_KAKARI_DIVISION_CODE + ";" + "kbl_new_belonging_limit_flag=" + v.KBL_NEW_BELONGING_LIMIT_FLAG + ";" + "location_code=" + v.LOCATION_CODE + Environment.NewLine;
                    }
                    System.Diagnostics.Debug.WriteLine(resUserlists);

                    //List<T>をIQueryable<T>に変換する。
                    //return Userlists.AsQueryable();
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(resUserlists.ToString(), Encoding.GetEncoding(932), "text/plain");

                    return response;
                }
            }
        }

        // 引数に社外者フラグがある場合 ⇒ 社外者
        //public IQueryable<BPUserInfoView> GetBPInfo(string uid)
        public HttpResponseMessage GetBPInfo(string uid)
        {
            // システム名（User-Agent）読み取り
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string user_agent = string.Empty;
            if (headers.Contains("User-Agent"))
            {
                user_agent = headers.GetValues("User-Agent").First();
            }

            // User-Agentが管理テーブルのシステム名として登録されているか確認
            using (var context_mainte = new JINJI_MAINTEEntities2())
            {
                // 出力>デバッグにSQLを表示
                context_mainte.Database.Log = x => System.Diagnostics.Debug.WriteLine(x);
                var list = context_mainte.REPLY_PERMISSION
                    .Where(x => x.DELETE_FLAG == false)
                    .GroupJoin(context_mainte.SYSTEM_INFORMATION, a => a.SYSTEM_CODE, b => b.SYSTEM_CODE, (a, b) => new { a, b })
                    .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new
                    {
                        a.a.SYSTEM_CODE,
                        a.a.RETURN_ITEM_CODE,
                        b.SYSTEM_NAME,
                        b.USER_AGENT
                    })
                    .GroupJoin(context_mainte.RETURN_ITEM, ab => ab.RETURN_ITEM_CODE, c => c.RETURN_ITEM_CODE, (ab, c) => new { ab, c })
                    .SelectMany(x => x.c.DefaultIfEmpty(), (ab, c) => new
                    {
                        ab.ab.SYSTEM_CODE,
                        ab.ab.RETURN_ITEM_CODE,
                        ab.ab.SYSTEM_NAME,
                        ab.ab.USER_AGENT,
                        c.RETURN_TABLE_NAME,
                        c.RETURN_ITEM_NAME
                    })
                    .Where(a => a.USER_AGENT == user_agent)
                    .Where(a => a.RETURN_TABLE_NAME == "PERSON_OF_OTHER_COMPANY")
                    .OrderBy(x => x.RETURN_ITEM_CODE);

                if (list.Any())
                {
                    //foreach (var p in list)
                    //    System.Diagnostics.Debug.WriteLine(
                    //        "System Code: {0}\nReturn Item Code: {1}\nSystem Name: {2}\nUser-Agent: {3}\nReturn Table Name: {4}\nReturn Item Name: {5}",
                    //p.SYSTEM_CODE, p.RETURN_ITEM_CODE, p.SYSTEM_NAME, p.USER_AGENT, p.RETURN_TABLE_NAME, p.RETURN_ITEM_NAME);
                }
                else
                {
                    var resUserAgent = this.Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "システム名が不正です");
                    resUserAgent.Content = new StringContent("システム名が不正です", Encoding.GetEncoding(932), "text/plain");
                    return resUserAgent;
                    //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                // 出力>デバッグに件数を表示
                System.Diagnostics.Debug.WriteLine("応答許可数:{0}件", list.Count());

                //List<BPUserInfoView> BPlists;
                //BPlists = null;
                using (var context = new JINJIEntities1())
                {
                    var BPlists = from emp in context.V_PERSON_OF_OTHER_COMPANY.AsNoTracking()
                        where emp.STAFF_CODE == uid
                        //select new BPUserInfoView
                        select new
                        {
                            STAFF_CODE = emp.STAFF_CODE,
                            KANJI_NAME = emp.KANJI_NAME,
                            ROMAN_NAME = emp.ROMAN_NAME,
                            KANA_NAME = emp.KANA_NAME,
                            STAFF_DIVISION_CODE = emp.STAFF_DIVISION_CODE,
                            ACCEPTANCE_BELONGING_CODE = emp.ACCEPTANCE_BELONGING_CODE,
                            BAB_BU_CODE = emp.BAB_BU_CODE,
                            BAB_START_VALIDITY_DATE = emp.BAB_START_VALIDITY_DATE,
                            BAB_END_VALIDITY_DATE = emp.BAB_END_VALIDITY_DATE,
                            BAB_BU_NAME_1 = emp.BAB_BU_NAME_1,
                            BAB_BU_NAME_2 = emp.BAB_BU_NAME_2,
                            BAB_BU_NAME_3 = emp.BAB_BU_NAME_3,
                            BAB_BU_INFORMAL_NAME = emp.BAB_BU_INFORMAL_NAME,
                            BAB_BU_ALTERATION_MARK = emp.BAB_BU_ALTERATION_MARK,
                            BAB_SECTION_CODE = emp.BAB_SECTION_CODE,
                            BAB_BU_LOCATION_CODE = emp.BAB_BU_LOCATION_CODE,
                            BAB_BU_NUMBER = emp.BAB_BU_NUMBER,
                            BAB_EXECUTIVE_NUMBER = emp.BAB_EXECUTIVE_NUMBER,
                            BAB_SHITSU_PERSON_SECTION = emp.BAB_SHITSU_PERSON_SECTION,
                            BAB_ADJUST_SECTION_NUMBER_A = emp.BAB_ADJUST_SECTION_NUMBER_A,
                            BAB_ADJUST_SECTION_NUMBER_B = emp.BAB_ADJUST_SECTION_NUMBER_B,
                            BAB_ADJUST_SECTION_NUMBER_C = emp.BAB_ADJUST_SECTION_NUMBER_C,
                            BAB_ADJUST_SECTION_NUMBER_D = emp.BAB_ADJUST_SECTION_NUMBER_D,
                            BAB_ASSESSMENT_CODE = emp.BAB_ASSESSMENT_CODE,
                            BAB_RANK_RISE_CODE = emp.BAB_RANK_RISE_CODE,
                            BAB_CHANGE_CODE = emp.BAB_CHANGE_CODE,
                            BAB_TRAINING_CODE = emp.BAB_TRAINING_CODE,
                            BAB_EMPLOY_OFFICE_CODE = emp.BAB_EMPLOY_OFFICE_CODE,
                            BAB_NEW_BELONGING_LIMIT_FLAG = emp.BAB_NEW_BELONGING_LIMIT_FLAG,
                            BAB_PERSONNEL_EXCEPTION = emp.BAB_PERSONNEL_EXCEPTION,
                            BAB_PERSONNEL_COMPULSION = emp.BAB_PERSONNEL_COMPULSION,
                            BAB_ENGLISH_NAME = emp.BAB_ENGLISH_NAME,
                            SAB_SHITSU_CODE = emp.SAB_SHITSU_CODE,
                            SAB_START_VALIDITY_DATE = emp.SAB_START_VALIDITY_DATE,
                            SAB_END_VALIDITY_DATE = emp.SAB_END_VALIDITY_DATE,
                            SAB_BU_CODE = emp.SAB_BU_CODE,
                            SAB_BU_NAME_1 = emp.SAB_BU_NAME_1,
                            SAB_BU_NAME_2 = emp.SAB_BU_NAME_2,
                            SAB_BU_INFORMAL_NAME = emp.SAB_BU_INFORMAL_NAME,
                            SAB_SHITSU_NAME = emp.SAB_SHITSU_NAME,
                            SAB_SHITSU_INFORMAL_NAME = emp.SAB_SHITSU_INFORMAL_NAME,
                            SAB_SHITSU_CHANGE_MARK = emp.SAB_SHITSU_CHANGE_MARK,
                            SAB_SHITSU_LOCATION_CODE = emp.SAB_SHITSU_LOCATION_CODE,
                            SAB_SHITSU_KA_NUMBER = emp.SAB_SHITSU_KA_NUMBER,
                            SAB_SHITSU_KA_DIVISION_CODE = emp.SAB_SHITSU_KA_DIVISION_CODE,
                            SAB_NEW_BELONGING_LIMIT_FLAG = emp.SAB_NEW_BELONGING_LIMIT_FLAG,
                            SAB_ENGLISH_NAME = emp.SAB_ENGLISH_NAME,
                            KAB_KAKARI_CODE = emp.KAB_KAKARI_CODE,
                            KAB_START_VALIDITY_DATE = emp.KAB_START_VALIDITY_DATE,
                            KAB_END_VALIDITY_DATE = emp.KAB_END_VALIDITY_DATE,
                            KAB_BU_CODE = emp.KAB_BU_CODE,
                            KAB_SHITSU_CODE = emp.KAB_SHITSU_CODE,
                            KAB_BU_NAME_1 = emp.KAB_BU_NAME_1,
                            KAB_BU_NAME_2 = emp.KAB_BU_NAME_2,
                            KAB_BU_INFORMAL_NAME = emp.KAB_BU_INFORMAL_NAME,
                            KAB_SHITSU_NAME = emp.KAB_SHITSU_NAME,
                            KAB_SHITSU_INFORMAL_NAME = emp.KAB_SHITSU_INFORMAL_NAME,
                            KAB_KAKARI_NAME = emp.KAB_KAKARI_NAME,
                            KAB_KAKARI_INFORMAL_NAME = emp.KAB_KAKARI_INFORMAL_NAME,
                            KAB_KAKARI_LOCATION_CODE = emp.KAB_KAKARI_LOCATION_CODE,
                            KAB_KAKARI_NUMBER = emp.KAB_KAKARI_NUMBER,
                            KAB_KAKARI_DIVISION_CODE = emp.KAB_KAKARI_DIVISION_CODE,
                            KAB_NEW_BELONGING_LIMIT_FLAG = emp.KAB_NEW_BELONGING_LIMIT_FLAG,
                            START_CONTRACT_TERM = emp.START_CONTRACT_TERM,
                            END_CONTRACT_TERM = emp.END_CONTRACT_TERM,
                            WORKING_PLACE_CODE = emp.WORKING_PLACE_CODE,
                            BELONGING_LATEST_CODE = emp.BELONGING_LATEST_CODE,
                            BBL_BU_CODE = emp.BBL_BU_CODE,
                            BBL_START_VALIDITY_DATE = emp.BBL_START_VALIDITY_DATE,
                            BBL_END_VALIDITY_DATE = emp.BBL_END_VALIDITY_DATE,
                            BBL_BU_NAME_1 = emp.BBL_BU_NAME_1,
                            BBL_BU_NAME_2 = emp.BBL_BU_NAME_2,
                            BBL_BU_NAME_3 = emp.BBL_BU_NAME_3,
                            BBL_BU_INFORMAL_NAME = emp.BBL_BU_INFORMAL_NAME,
                            BBL_BU_ALTERATION_MARK = emp.BBL_BU_ALTERATION_MARK,
                            BBL_SECTION_CODE = emp.BBL_SECTION_CODE,
                            BBL_BU_LOCATION_CODE = emp.BBL_BU_LOCATION_CODE,
                            BBL_BU_NUMBER = emp.BBL_BU_NUMBER,
                            BBL_EXECUTIVE_NUMBER = emp.BBL_EXECUTIVE_NUMBER,
                            BBL_SHITSU_PERSON_SECTION = emp.BBL_SHITSU_PERSON_SECTION,
                            BBL_ADJUST_SECTION_NUMBER_A = emp.BBL_ADJUST_SECTION_NUMBER_A,
                            BBL_ADJUST_SECTION_NUMBER_B = emp.BBL_ADJUST_SECTION_NUMBER_B,
                            BBL_ADJUST_SECTION_NUMBER_C = emp.BBL_ADJUST_SECTION_NUMBER_C,
                            BBL_ADJUST_SECTION_NUMBER_D = emp.BBL_ADJUST_SECTION_NUMBER_D,
                            BBL_ASSESSMENT_CODE = emp.BBL_ASSESSMENT_CODE,
                            BBL_RANK_RISE_CODE = emp.BBL_RANK_RISE_CODE,
                            BBL_CHANGE_CODE = emp.BBL_CHANGE_CODE,
                            BBL_TRAINING_CODE = emp.BBL_TRAINING_CODE,
                            BBL_EMPLOY_OFFICE_CODE = emp.BBL_EMPLOY_OFFICE_CODE,
                            BBL_NEW_BELONGING_LIMIT_FLAG = emp.BBL_NEW_BELONGING_LIMIT_FLAG,
                            BBL_PERSONNEL_EXCEPTION = emp.BBL_PERSONNEL_EXCEPTION,
                            BBL_PERSONNEL_COMPULSION = emp.BBL_PERSONNEL_COMPULSION,
                            BBL_ENGLISH_NAME = emp.BBL_ENGLISH_NAME,
                            SBL_SHITSU_CODE = emp.SBL_SHITSU_CODE,
                            SBL_START_VALIDITY_DATE = emp.SBL_START_VALIDITY_DATE,
                            SBL_END_VALIDITY_DATE = emp.SBL_END_VALIDITY_DATE,
                            SBL_BU_CODE = emp.SBL_BU_CODE,
                            SBL_BU_NAME_1 = emp.SBL_BU_NAME_1,
                            SBL_BU_NAME_2 = emp.SBL_BU_NAME_2,
                            SBL_BU_INFORMAL_NAME = emp.SBL_BU_INFORMAL_NAME,
                            SBL_SHITSU_NAME = emp.SBL_SHITSU_NAME,
                            SBL_SHITSU_INFORMAL_NAME = emp.SBL_SHITSU_INFORMAL_NAME,
                            SBL_SHITSU_CHANGE_MARK = emp.SBL_SHITSU_CHANGE_MARK,
                            SBL_SHITSU_LOCATION_CODE = emp.SBL_SHITSU_LOCATION_CODE,
                            SBL_SHITSU_KA_NUMBER = emp.SBL_SHITSU_KA_NUMBER,
                            SBL_SHITSU_KA_DIVISION_CODE = emp.SBL_SHITSU_KA_DIVISION_CODE,
                            SBL_NEW_BELONGING_LIMIT_FLAG = emp.SBL_NEW_BELONGING_LIMIT_FLAG,
                            SBL_ENGLISH_NAME = emp.SBL_ENGLISH_NAME,
                            KBL_KAKARI_CODE = emp.KBL_KAKARI_CODE,
                            KBL_START_VALIDITY_DATE = emp.KBL_START_VALIDITY_DATE,
                            KBL_END_VALIDITY_DATE = emp.KBL_END_VALIDITY_DATE,
                            KBL_BU_CODE = emp.KBL_BU_CODE,
                            KBL_SHITSU_CODE = emp.KBL_SHITSU_CODE,
                            KBL_BU_NAME_1 = emp.KBL_BU_NAME_1,
                            KBL_BU_NAME_2 = emp.KBL_BU_NAME_2,
                            KBL_BU_INFORMAL_NAME = emp.KBL_BU_INFORMAL_NAME,
                            KBL_SHITSU_NAME = emp.KBL_SHITSU_NAME,
                            KBL_SHITSU_INFORMAL_NAME = emp.KBL_SHITSU_INFORMAL_NAME,
                            KBL_KAKARI_NAME = emp.KBL_KAKARI_NAME,
                            KBL_KAKARI_INFORMAL_NAME = emp.KBL_KAKARI_INFORMAL_NAME,
                            KBL_KAKARI_LOCATION_CODE = emp.KBL_KAKARI_LOCATION_CODE,
                            KBL_KAKARI_NUMBER = emp.KBL_KAKARI_NUMBER,
                            KBL_KAKARI_DIVISION_CODE = emp.KBL_KAKARI_DIVISION_CODE,
                            KBL_NEW_BELONGING_LIMIT_FLAG = emp.KBL_NEW_BELONGING_LIMIT_FLAG,
                            ORIGINAL_CONTRACTOR_CODE = emp.ORIGINAL_CONTRACTOR_CODE,
                            ORIGINAL_CONTRACTOR_NAME = emp.ORIGINAL_CONTRACTOR_NAME,
                            BELONGING_COMPANY_CODE = emp.BELONGING_COMPANY_CODE,
                            BELONGING_COMPANY_NAME = emp.BELONGING_COMPANY_NAME,
                            CARD_STATUS = emp.CARD_STATUS
                            //}).ToList();
                        };
                    if (BPlists.Any() == false)
                    {
                        //throw new HttpResponseException(HttpStatusCode.NotFound);
                        var resUserList = this.Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "エラーメッセージ");
                        resUserList.Content = new StringContent("error_code=E0001", Encoding.GetEncoding(932), "text/plain");
                        return resUserList;
                    }

                    // 出力>デバッグにSQLを表示
                    //context.Database.Log = x => System.Diagnostics.Debug.WriteLine(x);
                    System.Diagnostics.Debug.WriteLine("従業員情報:{0}件", BPlists.Count());
                    System.Diagnostics.Debug.WriteLine(user_agent);

                    // 応答許可項目文字列作成
                    string resBPlists = string.Empty;
                    foreach (var v in BPlists)
                    {
                        //System.Diagnostics.Debug.WriteLine($"{v.STAFF_CODE + ":",-15}{v.KANJI_NAME}");
                        foreach (var p in list)
                        {
                            switch (p.RETURN_ITEM_NAME)
                            {
                                case "STAFF_CODE":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.STAFF_CODE + ";";
                                    break;
                                case "KANJI_NAME":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.KANJI_NAME + ";";
                                    break;
                                case "ROMAN_NAME":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ROMAN_NAME + ";";
                                    break;
                                case "KANA_NAME":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.KANA_NAME + ";";
                                    break;
                                case "STAFF_DIVISION_CODE":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.STAFF_DIVISION_CODE + ";";
                                    break;
                                case "ACCEPTANCE_BELONGING_CODE":
                                    resBPlists = resBPlists
                                        + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ACCEPTANCE_BELONGING_CODE + ";"
                                        + "bab_bu_code=" + v.BAB_BU_CODE + ";"
                                        + "bab_start_validity_date=" + v.BAB_START_VALIDITY_DATE + ";"
                                        + "bab_end_validity_date=" + v.BAB_END_VALIDITY_DATE + ";"
                                        + "bab_bu_name_1=" + v.BAB_BU_NAME_1 + ";"
                                        + "bab_bu_name_2=" + v.BAB_BU_NAME_2 + ";"
                                        + "bab_bu_name_3=" + v.BAB_BU_NAME_3 + ";"
                                        + "bab_bu_informal_name=" + v.BAB_BU_INFORMAL_NAME + ";"
                                        + "bab_bu_alteration_mark=" + v.BAB_BU_ALTERATION_MARK + ";"
                                        + "bab_section_code=" + v.BAB_SECTION_CODE + ";"
                                        + "bab_bu_location_code=" + v.BAB_BU_LOCATION_CODE + ";"
                                        + "bab_bu_number=" + v.BAB_BU_NUMBER + ";"
                                        + "bab_executive_number=" + v.BAB_EXECUTIVE_NUMBER + ";"
                                        + "bab_shitsu_person_section=" + v.BAB_SHITSU_PERSON_SECTION + ";"
                                        + "bab_adjust_section_number_a=" + v.BAB_ADJUST_SECTION_NUMBER_A + ";"
                                        + "bab_adjust_section_number_b=" + v.BAB_ADJUST_SECTION_NUMBER_B + ";"
                                        + "bab_adjust_section_number_c=" + v.BAB_ADJUST_SECTION_NUMBER_C + ";"
                                        + "bab_adjust_section_number_d=" + v.BAB_ADJUST_SECTION_NUMBER_D + ";"
                                        + "bab_assessment_code=" + v.BAB_ASSESSMENT_CODE + ";"
                                        + "bab_rank_rise_code=" + v.BAB_RANK_RISE_CODE + ";"
                                        + "bab_change_code=" + v.BAB_CHANGE_CODE + ";"
                                        + "bab_training_code=" + v.BAB_TRAINING_CODE + ";"
                                        + "bab_employ_office_code=" + v.BAB_EMPLOY_OFFICE_CODE + ";"
                                        + "bab_new_belonging_limit_flag=" + v.BAB_NEW_BELONGING_LIMIT_FLAG + ";"
                                        + "bab_personnel_exception=" + v.BAB_PERSONNEL_EXCEPTION + ";"
                                        + "bab_personnel_compulsion=" + v.BAB_PERSONNEL_COMPULSION + ";"
                                        + "bab_english_name=" + v.BAB_ENGLISH_NAME + ";"
                                        + "sab_shitsu_code=" + v.SAB_SHITSU_CODE + ";"
                                        + "sab_start_validity_date=" + v.SAB_START_VALIDITY_DATE + ";"
                                        + "sab_end_validity_date=" + v.SAB_END_VALIDITY_DATE + ";"
                                        + "sab_bu_code=" + v.SAB_BU_CODE + ";"
                                        + "sab_bu_name_1=" + v.SAB_BU_NAME_1 + ";"
                                        + "sab_bu_name_2=" + v.SAB_BU_NAME_2 + ";"
                                        + "sab_bu_informal_name=" + v.SAB_BU_INFORMAL_NAME + ";"
                                        + "sab_shitsu_name=" + v.SAB_SHITSU_NAME + ";"
                                        + "sab_shitsu_informal_name=" + v.SAB_SHITSU_INFORMAL_NAME + ";"
                                        + "sab_shitsu_change_mark=" + v.SAB_SHITSU_CHANGE_MARK + ";"
                                        + "sab_shitsu_location_code=" + v.SAB_SHITSU_LOCATION_CODE + ";"
                                        + "sab_shitsu_ka_number=" + v.SAB_SHITSU_KA_NUMBER + ";"
                                        + "sab_shitsu_ka_division_code=" + v.SAB_SHITSU_KA_DIVISION_CODE + ";"
                                        + "sab_new_belonging_limit_flag=" + v.SAB_NEW_BELONGING_LIMIT_FLAG + ";"
                                        + "sab_english_name=" + v.SAB_ENGLISH_NAME + ";"
                                        + "kab_kakari_code=" + v.KAB_KAKARI_CODE + ";"
                                        + "kab_start_validity_date=" + v.KAB_START_VALIDITY_DATE + ";"
                                        + "kab_end_validity_date=" + v.KAB_END_VALIDITY_DATE + ";"
                                        + "kab_bu_code=" + v.KAB_BU_CODE + ";"
                                        + "kab_shitsu_code=" + v.KAB_SHITSU_CODE + ";"
                                        + "kab_bu_name_1=" + v.KAB_BU_NAME_1 + ";"
                                        + "kab_bu_name_2=" + v.KAB_BU_NAME_2 + ";"
                                        + "kab_bu_informal_name=" + v.KAB_BU_INFORMAL_NAME + ";"
                                        + "kab_shitsu_name=" + v.KAB_SHITSU_NAME + ";"
                                        + "kab_shitsu_informal_name=" + v.KAB_SHITSU_INFORMAL_NAME + ";"
                                        + "kab_kakari_name=" + v.KAB_KAKARI_NAME + ";"
                                        + "kab_kakari_informal_name=" + v.KAB_KAKARI_INFORMAL_NAME + ";"
                                        + "kab_kakari_location_code=" + v.KAB_KAKARI_LOCATION_CODE + ";"
                                        + "kab_kakari_number=" + v.KAB_KAKARI_NUMBER + ";"
                                        + "kab_kakari_division_code=" + v.KAB_KAKARI_DIVISION_CODE + ";"
                                        + "kab_new_belonging_limit_flag=" + v.KAB_NEW_BELONGING_LIMIT_FLAG + ";"
                                        ;
                                    break;
                                case "START_CONTRACT_TERM":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.START_CONTRACT_TERM + ";";
                                    break;
                                case "END_CONTRACT_TERM":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.END_CONTRACT_TERM + ";";
                                    break;
                                case "WORKING_PLACE_CODE":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.WORKING_PLACE_CODE + ";";
                                    break;
                                case "BELONGING_LATEST_CODE":
                                    resBPlists = resBPlists
                                        + p.RETURN_ITEM_NAME.ToLower() + "=" + v.BELONGING_LATEST_CODE + ";"
                                        + "bbl_bu_code=" + v.BBL_BU_CODE + ";"
                                        + "bbl_start_validity_date=" + v.BBL_START_VALIDITY_DATE + ";"
                                        + "bbl_end_validity_date=" + v.BBL_END_VALIDITY_DATE + ";"
                                        + "bbl_bu_name_1=" + v.BBL_BU_NAME_1 + ";"
                                        + "bbl_bu_name_2=" + v.BBL_BU_NAME_2 + ";"
                                        + "bbl_bu_name_3=" + v.BBL_BU_NAME_3 + ";"
                                        + "bbl_bu_informal_name=" + v.BBL_BU_INFORMAL_NAME + ";"
                                        + "bbl_bu_alteration_mark=" + v.BBL_BU_ALTERATION_MARK + ";"
                                        + "bbl_section_code=" + v.BBL_SECTION_CODE + ";"
                                        + "bbl_bu_location_code=" + v.BBL_BU_LOCATION_CODE + ";"
                                        + "bbl_bu_number=" + v.BBL_BU_NUMBER + ";"
                                        + "bbl_executive_number=" + v.BBL_EXECUTIVE_NUMBER + ";"
                                        + "bbl_shitsu_person_section=" + v.BBL_SHITSU_PERSON_SECTION + ";"
                                        + "bbl_adjust_section_number_a=" + v.BBL_ADJUST_SECTION_NUMBER_A + ";"
                                        + "bbl_adjust_section_number_b=" + v.BBL_ADJUST_SECTION_NUMBER_B + ";"
                                        + "bbl_adjust_section_number_c=" + v.BBL_ADJUST_SECTION_NUMBER_C + ";"
                                        + "bbl_adjust_section_number_d=" + v.BBL_ADJUST_SECTION_NUMBER_D + ";"
                                        + "bbl_assessment_code=" + v.BBL_ASSESSMENT_CODE + ";"
                                        + "bbl_rank_rise_code=" + v.BBL_RANK_RISE_CODE + ";"
                                        + "bbl_change_code=" + v.BBL_CHANGE_CODE + ";"
                                        + "bbl_training_code=" + v.BBL_TRAINING_CODE + ";"
                                        + "bbl_employ_office_code=" + v.BBL_EMPLOY_OFFICE_CODE + ";"
                                        + "bbl_new_belonging_limit_flag=" + v.BBL_NEW_BELONGING_LIMIT_FLAG + ";"
                                        + "bbl_personnel_exception=" + v.BBL_PERSONNEL_EXCEPTION + ";"
                                        + "bbl_personnel_compulsion=" + v.BBL_PERSONNEL_COMPULSION + ";"
                                        + "bbl_english_name=" + v.BBL_ENGLISH_NAME + ";"
                                        + "sbl_shitsu_code=" + v.SBL_SHITSU_CODE + ";"
                                        + "sbl_start_validity_date=" + v.SBL_START_VALIDITY_DATE + ";"
                                        + "sbl_end_validity_date=" + v.SBL_END_VALIDITY_DATE + ";"
                                        + "sbl_bu_code=" + v.SBL_BU_CODE + ";"
                                        + "sbl_bu_name_1=" + v.SBL_BU_NAME_1 + ";"
                                        + "sbl_bu_name_2=" + v.SBL_BU_NAME_2 + ";"
                                        + "sbl_bu_informal_name=" + v.SBL_BU_INFORMAL_NAME + ";"
                                        + "sbl_shitsu_name=" + v.SBL_SHITSU_NAME + ";"
                                        + "sbl_shitsu_informal_name=" + v.SBL_SHITSU_INFORMAL_NAME + ";"
                                        + "sbl_shitsu_change_mark=" + v.SBL_SHITSU_CHANGE_MARK + ";"
                                        + "sbl_shitsu_location_code=" + v.SBL_SHITSU_LOCATION_CODE + ";"
                                        + "sbl_shitsu_ka_number=" + v.SBL_SHITSU_KA_NUMBER + ";"
                                        + "sbl_shitsu_ka_division_code=" + v.SBL_SHITSU_KA_DIVISION_CODE + ";"
                                        + "sbl_new_belonging_limit_flag=" + v.SBL_NEW_BELONGING_LIMIT_FLAG + ";"
                                        + "sbl_english_name=" + v.SBL_ENGLISH_NAME + ";"
                                        + "kbl_kakari_code=" + v.KBL_KAKARI_CODE + ";"
                                        + "kbl_start_validity_date=" + v.KBL_START_VALIDITY_DATE + ";"
                                        + "kbl_end_validity_date=" + v.KBL_END_VALIDITY_DATE + ";"
                                        + "kbl_bu_code=" + v.KBL_BU_CODE + ";"
                                        + "kbl_shitsu_code=" + v.KBL_SHITSU_CODE + ";"
                                        + "kbl_bu_name_1=" + v.KBL_BU_NAME_1 + ";"
                                        + "kbl_bu_name_2=" + v.KBL_BU_NAME_2 + ";"
                                        + "kbl_bu_informal_name=" + v.KBL_BU_INFORMAL_NAME + ";"
                                        + "kbl_shitsu_name=" + v.KBL_SHITSU_NAME + ";"
                                        + "kbl_shitsu_informal_name=" + v.KBL_SHITSU_INFORMAL_NAME + ";"
                                        + "kbl_kakari_name=" + v.KBL_KAKARI_NAME + ";"
                                        + "kbl_kakari_informal_name=" + v.KBL_KAKARI_INFORMAL_NAME + ";"
                                        + "kbl_kakari_location_code=" + v.KBL_KAKARI_LOCATION_CODE + ";"
                                        + "kbl_kakari_number=" + v.KBL_KAKARI_NUMBER + ";"
                                        + "kbl_kakari_division_code=" + v.KBL_KAKARI_DIVISION_CODE + ";"
                                        + "kbl_new_belonging_limit_flag=" + v.KBL_NEW_BELONGING_LIMIT_FLAG + ";"
                                        ;
                                    break;
                                case "ORIGINAL_CONTRACTOR_CODE":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ORIGINAL_CONTRACTOR_CODE + ";";
                                    break;
                                case "ORIGINAL_CONTRACTOR_NAME":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.ORIGINAL_CONTRACTOR_NAME + ";";
                                    break;
                                case "BELONGING_COMPANY_CODE":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.BELONGING_COMPANY_CODE + ";";
                                    break;
                                case "BELONGING_COMPANY_NAME":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.BELONGING_COMPANY_NAME + ";";
                                    break;
                                case "CARD_STATUS":
                                    resBPlists = resBPlists + p.RETURN_ITEM_NAME.ToLower() + "=" + v.CARD_STATUS + ";";
                                    break;
                            }
                        }

                        resBPlists = resBPlists.Substring(0, resBPlists.Length - 1) + Environment.NewLine;
                    }
                    System.Diagnostics.Debug.WriteLine(resBPlists);

                    //List<T>をIQueryable<T>に変換する。
                    //return BPlists.AsQueryable();
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(resBPlists.ToString(), Encoding.GetEncoding(932), "text/plain");

                    return response;
                }
            }
        }
    }
}
