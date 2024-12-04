namespace Sales.Data.Events
{
    public class EventoSale
    {
        public int IdSale { get; set; }
        public string NumberSale { get; set; }
        public DateTime DataCriacao { get; set; }
        public TipoEvento Tipo { get; set; }
    }

    public enum TipoEvento
    {
        CompraCriada,
        CompraAlterada,
        CompraCancelada
    }
}
