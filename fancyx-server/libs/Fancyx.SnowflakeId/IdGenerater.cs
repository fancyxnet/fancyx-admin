using System;
using System.Threading;

namespace Fancyx.SnowflakeId
{
    public class IdGenerater
    {
        private static readonly DateTime unixTime = new DateTime(1970, 1, 1, 0, 0, 0);
        private static long machineId;//机器ID
        private static long datacenterId = 0L;//数据ID
        private static long sequence = 0L;//序列号,计数从零开始

        private static readonly long twepoch = 687888001020L; //起始的时间戳，唯一时间变量，这是一个避免重复的随机量，自行设定不要大于当前时间戳

        private static readonly long machineIdBits = 5L; //机器码字节数
        private static readonly long datacenterIdBits = 5L; //数据字节数
        private static readonly long maxMachineId = -1L ^ -1L << (int)machineIdBits; //最大机器ID
        private static readonly long maxDatacenterId = -1L ^ -1L << (int)datacenterIdBits;//最大数据ID

        private static readonly long sequenceBits = 12L; //计数器字节数，12个字节用来保存计数码
        private static readonly long machineIdShift = sequenceBits; //机器码数据左移位数，就是后面计数器占用的位数
        private static readonly long datacenterIdShift = sequenceBits + machineIdBits; //数据中心码数据左移位数
        private static readonly long timestampLeftShift = sequenceBits + machineIdBits + datacenterIdBits; //时间戳左移动位数就是机器码+计数器总字节数+数据字节数
        private static readonly long sequenceMask = -1L ^ -1L << (int)sequenceBits; //一毫秒内可以产生计数，如果达到该值则等到下一毫秒在进行生成
        private static long lastTimestamp = -1L;//最后时间戳

        private static readonly object syncRoot = new object(); //加锁对象

        private static Lazy<IdGenerater> Lazy => new Lazy<IdGenerater>(() => new IdGenerater());
        public static IdGenerater Instance => Lazy.Value;

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="machineId">机器Id</param>
        /// <param name="datacenterId">数据中心Id</param>
        public static void Init(short machineId, short datacenterId)
        {
            if (machineId < 0 || machineId > maxMachineId)
            {
                throw new ArgumentOutOfRangeException($"The machineId is illegal! => Range interval [0,{maxMachineId}]");
            }
            else
            {
                IdGenerater.machineId = machineId;
            }

            if (datacenterId < 0 || datacenterId > maxDatacenterId)
            {
                throw new ArgumentOutOfRangeException($"The datacenterId is illegal! => Range interval [0,{maxDatacenterId}]");
            }
            else
            {
                IdGenerater.datacenterId = datacenterId;
            }
        }

        /// <summary>
        /// 获取下一毫秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns>时间戳:毫秒</returns>
        private static long GetNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetMsTimestamp();
            int count = 0;
            while (timestamp <= lastTimestamp)//这里获取新的时间,可能会有错,这算法与comb一样对机器时间的要求很严格
            {
                count++;
                if (count > 10) throw new Exception("The machine may not get the right time.");
                Thread.Sleep(1);
                timestamp = GetMsTimestamp();
            }
            return timestamp;
        }

        /// <summary>
        /// 获取长整形的ID
        /// </summary>
        /// <returns>分布式Id</returns>
        public long NextId()
        {
            lock (syncRoot)
            {
                long timestamp = GetMsTimestamp();
                if (lastTimestamp == timestamp)
                {
                    //同一毫秒中生成ID
                    sequence = sequence + 1 & sequenceMask; //用&运算计算该毫秒内产生的计数是否已经到达上限
                    if (sequence == 0)
                    {
                        //一毫秒内产生的ID计数已达上限，等待下一毫秒
                        timestamp = GetNextTimestamp(lastTimestamp);
                    }
                }
                else
                {
                    //不同毫秒生成ID
                    sequence = 0L; //计数清0
                }
                if (timestamp < lastTimestamp)
                {
                    //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                    throw new Exception($"Clock moved backwards.  Refusing to generate id for {lastTimestamp - timestamp} milliseconds!");
                }
                lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                long id = timestamp - twepoch << (int)timestampLeftShift
                    | datacenterId << (int)datacenterIdShift
                    | machineId << (int)machineIdShift
                    | sequence;
                return id;
            }
        }

        private static long GetMsTimestamp()
        {
            return (long)(DateTime.Now.ToUniversalTime() - unixTime).TotalMilliseconds;
        }
    }
}