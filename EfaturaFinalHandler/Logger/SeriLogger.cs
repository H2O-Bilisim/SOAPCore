using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfaturaFinalHandler.Logger
{
    public class SeriLogger
    {
        //public static string MongoConnectionPath = "mongodb://SignatureQ:SignatureQUSer@www.leverage.com.tr:43222/?authSource=Signature";
        public static string MongoConnectionPath = "mongodb://10.10.50.6:27017";
        public static string MongoDbName = "LogDb";
        public static string MongoCollectionName = "SOAPCoreLogs";
        public Serilog.Core.Logger Log;

        public SeriLogger()
        {
            Log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.MongoDBBson(cfg =>
                {
                    var mongoDbSettings = MongoClientSettings.FromConnectionString(MongoConnectionPath);
                    var mongoDbInstance = new MongoClient(mongoDbSettings).GetDatabase(MongoDbName);

                    // sink will use the IMongoDatabase instance provided
                    cfg.SetMongoDatabase(mongoDbInstance);
                    cfg.SetCollectionName(MongoCollectionName);
                    cfg.SetBatchPostingLimit(1);
                    cfg.SetBatchPeriod(new TimeSpan(1));
                })
                .CreateLogger();
        }
    }
}
