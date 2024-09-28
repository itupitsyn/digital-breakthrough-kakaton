namespace RecSysConverter.TrainSet
{
    internal class DataSet
    {
        public long user_id { get; set; }
        public long positive_video_id1 { get; set; }
        public long positive_video_id2 { get; set; }
        public long negative_video_id { get; set; }
        public byte[] positive_video1_bert { get; set; }
        public byte[] positive_video2_bert { get; set; }
        public byte[] negative_video_bert { get; set; }
    }
}
