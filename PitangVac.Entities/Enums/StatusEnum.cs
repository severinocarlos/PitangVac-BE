namespace PitangVac.Entity.Enums
{
    public static class StatusEnum
    {
        public static readonly string Agendado = "Agendado";
        public static readonly string Concluído = "Concluído";
        public static readonly string Cancelado = "Cancelado";

        public static List<string> Status { get; set; } = new()
        {
            Agendado,
            Concluído,
            Cancelado
        };
    }
}
