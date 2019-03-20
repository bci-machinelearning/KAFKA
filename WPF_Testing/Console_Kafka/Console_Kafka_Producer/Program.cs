using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;
using KafkaNet.Statistics;

namespace Console_Kafka
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Produce(GetKafkaBroker(), getTopicName());
                System.Threading.Thread.Sleep(3000);
            } while (true);
        }

        private static void Produce(string broker, string topic)
        {
            var options = new KafkaOptions(new Uri(broker));
            var router = new BrokerRouter(options);
            var client = new Producer(router);

            var currentDatetime = DateTime.Now;
            var key = currentDatetime.Second.ToString();
            var events = new[] { new Message("Hello World " + currentDatetime, key) };
            client.SendMessageAsync(topic, events).Wait(1500);
            Console.WriteLine("Produced: Key: {0}. Message: {1}", key, events[0].Value.ToUtf8String());

            using (client) { }
        }

        private static string GetKafkaBroker()
        {
            string KafkaBroker = string.Empty;
            const string kafkaBrokerKeyName = "KafkaBroker";

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(kafkaBrokerKeyName))
            {
                KafkaBroker = "http://192.168.1.4:9092";
            }
            else
            {
                KafkaBroker = ConfigurationManager.AppSettings[kafkaBrokerKeyName];
            }
            return KafkaBroker;
        }
        private static string getTopicName()
        {
            string TopicName = string.Empty;
            //const string topicNameKeyName = "Topic";

            //if (!ConfigurationManager.AppSettings.AllKeys.Contains(topicNameKeyName))
            //{
            //    throw new Exception("Key \"" + topicNameKeyName + "\" not found in Config file -> configuration/AppSettings");
            //}
            //else
            //{
            //    TopicName = ConfigurationManager.AppSettings[topicNameKeyName];
            //}
            //return TopicName;
            return "test";
        }
    }
}
