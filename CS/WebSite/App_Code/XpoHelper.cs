using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

/// <summary>
/// Summary description for XpoHelper
/// </summary>
public static class XpoHelper {
    static XpoHelper() {
        CreateDefaultObjects();
    }

    public static Session GetNewSession() {
        return new Session(DataLayer);
    }

    public static UnitOfWork GetNewUnitOfWork() {
        return new UnitOfWork(DataLayer);
    }

    private readonly static object lockObject = new object();

    static IDataLayer fDataLayer;
    static IDataLayer DataLayer {
        get {
            if (fDataLayer == null) {
                lock (lockObject) {
                    fDataLayer = GetDataLayer();
                }
            }
            return fDataLayer;
        }
    }

    private static IDataLayer GetDataLayer() {
        XpoDefault.Session = null;

        InMemoryDataStore ds = new InMemoryDataStore();
        XPDictionary dict = new ReflectionDictionary();
        dict.GetDataStoreSchema(typeof(MyObject).Assembly);

        return new ThreadSafeDataLayer(dict, ds);
    }

    static void CreateDefaultObjects() {
        using (UnitOfWork uow = GetNewUnitOfWork()) {
            MyObject obj = new MyObject(uow);
            obj.Title = "Test1";
            obj.Message = "Message1";
            obj.IsRead = false;

            obj = new MyObject(uow);
            obj.Title = "Test2";
            obj.Message = "Message2";
            obj.IsRead = false;

            obj = new MyObject(uow);
            obj.Title = "Test3";
            obj.Message = "Message3";
            obj.IsRead = true;

            obj = new MyObject(uow);
            obj.Title = "Test4";
            obj.Message = "Message4";
            obj.IsRead = false;

            uow.CommitChanges();
        }
    }
}