using CacheManager.Core;
using FengYun.CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CacheManager.Test
{
    public class Person
    {
        public Person()
            : this("翟晓东", DateTime.Now, 27)
        { }
        public Person(string name, DateTime date, int age)
        {
            this.Name = name;
            this.BirthDay = date;
            this.Age = age;
        }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int Age { get; set; }
    }

    public class City
    {
        public City()
            : this("北京", 2022522)
        {

        }
        public City(string name, int count, DateTime? time = null)
        {
            this.Name = name;
            this.Persons = count;
            this.Crttime = time ?? DateTime.Now;
        }
        public string Name { get; set; }

        public int Persons { get; set; }

        public DateTime? Crttime { get; set; }

    }

    public partial class Index : System.Web.UI.Page
    {
        private List<City> citys = new List<City>()
        {
            new City("北京",2100000),
            new City("上海",2400000),
            new City("深圳",1100000),
            new City("苏州",1600000,DateTime.Now.AddDays(50)),
            new City("郑州",1600000,DateTime.Now.AddDays(20))
        };
        private ICacheManager<object> cacheObj = CacheManagerHelper.Default;
        protected void Page_Load(object sender, EventArgs e)
        {

            //cacheObj = CacheFactory.Build("getStartedCache", settings =>
            //{
            //    settings.WithSystemRuntimeCacheHandle("inProcessCache")
            //        .And
            //        .WithRedisConfiguration("redis",config=>
            //        {
            //            config.WithAllowAdmin()
            //            .WithDatabase(0)
            //            .WithEndpoint("172.16.200.131", 14343);
            //        })
            //        .WithMaxRetries(1000)//尝试次数
            //        .WithRetryTimeout(100)//尝试超时时间
            //        .WithRedisBackplane("redis")//redis使用Back Plate
            //        .WithRedisCacheHandle("redis", true);//redis缓存handle
            //});
            //cacheStr = CacheFactory.FromConfiguration<string>("multiCacheName");
            if (!IsPostBack)
            {

            }

        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            cacheObj.Add("user:1", new Person(), "LoginSessions");
            cacheObj.Add("user:2", new Person("张三丰", DateTime.Now.AddYears(-20), 22), "LoginSessions");
            cacheObj.Add("sysCitys", new List<dynamic>() { new { name = "北京", persons = 100000 }, new { name = "上海", persons = 150000 }, new { name = "苏州", persons = 200000 } });
            cacheObj.Add("citysInfo", citys);
            cacheObj.Add("objKeyA", DateTime.Now);
            cacheObj.Add("objKeyB", new Person());
            cacheObj.Add("objKeyC", "自定义字符串信息");
            //cacheObj.AddOrUpdate("user:1", "LoginSessions", new Person(), p => new Person("王麻子", DateTime.Now.AddDays(30), 33));
            //cacheObj.AddOrUpdate("counter", 0, v => (Int32)v + 1);
            var dicCache = CacheManagerHelper.DictionaryCache;
            dicCache.Add("dicPerson", new Person());
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            var user1 = cacheObj.Get("user:1", "LoginSessions");
            var user2 = cacheObj.Get<Person>("user:2", "LoginSessions");
            var sysCitys = cacheObj.GetCacheItem("sysCitys");
            var cityInfo = cacheObj.Get<List<City>>("citysInfo");
            var objkeyA = cacheObj.Get("objKeyA");
            var objkeyB = cacheObj.Get<Person>("objKeyB");
            var objkeyC = cacheObj.Get<string>("objKeyC");
            var dicCache = CacheManagerHelper.DictionaryCacheFor<object>();
            var dicPerson = dicCache.Get<Person>("dicPerson");
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            cacheObj.Remove("objKeyB");
            string msg = string.Format("objKeyB removed? {0}", (cacheObj.Get("objKeyB") == null).ToString());
            ClientScript.RegisterClientScriptBlock(typeof(Page), Guid.NewGuid().ToString(), "alert('" + msg + ");", true);
        }

    }
}