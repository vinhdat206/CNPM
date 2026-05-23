namespace CNPMFastFood.Models
{
    public class Setting
    {
        public int Id { get; set; }
        
        public string? LogoUrl { get; set; }

        public string StoreName { get; set; } = "";
        public string StoreEmail { get; set; } = "";
        public string StorePhone { get; set; } = "";
        public string StoreAddress { get; set; } = "";

        public string OpenTime { get; set; } = "08:00";
        public string CloseTime { get; set; } = "22:00";

        public decimal ShippingFee { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public int EstimatedDeliveryMinutes { get; set; }

        public bool IsCodEnabled { get; set; }
        public bool IsBankTransferEnabled { get; set; }
    }
}