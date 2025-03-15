[System.Serializable]
public class Usuario
{
    public string nombreJugador;
    public int puntaje;

    public Usuario(string nombre, int puntaje)
    {
        this.nombreJugador = nombre;
        this.puntaje = puntaje;
    }
}