using System;
using Api.Controllers;
using Hurace.Core.DAL.Domain;

namespace Api
{
    public class SplitTimeOutDto
    {
        public int RunNo { get; set; }
        public int SplitTimeNo { get; set; }
        public DateTime SplitTime { get; set; }

        public static SplitTimeOutDto FromSplitTime(SplitTime splitTime)
        {
            return new SplitTimeOutDto()
            {
                RunNo = splitTime.RunNo,
                SplitTime = splitTime.Time,
                SplitTimeNo = splitTime.SplittimeNo
            };
        }
    }
}