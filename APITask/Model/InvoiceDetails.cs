namespace APITask.Model
{
    public class InvoiceDetails
    {


        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Customer { get; set; }
        public string OfferNumber { get; set; }
        public string PartNumber { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Taxable { get; set; }
        public decimal LandedCost { get; set; }
        public decimal Profitability { get; set; }
        public string SalesExecutive { get; set; }
    }
}
