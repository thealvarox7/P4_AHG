using System.Collections.Generic;
using UnityEngine;

public class A1 : MonoBehaviour
{
    [SerializeField] private bool ejecutarEjemploEnStart = true;
    [SerializeField, Range(0f, 100f)] private float probabilidadCriticoEjemplo = 25f;

    private void Start()
    {
        if (!ejecutarEjemploEnStart)
        {
            return;
        }

        EjecutarEjemplo();
    }

    public void SimularAtaque(List<Enemy> enemigos)
    {
        if (enemigos == null || enemigos.Count == 0)
        {
            Debug.Log("No hay enemigos para simular.");
            return;
        }

        foreach (Enemy enemigo in enemigos)
        {
            float dano = enemigo.Atacar();
            Debug.Log($"{enemigo.Nombre} ataca y hace {dano} de dano.");
        }
    }

    public void AplicarDanioEnArea(List<Enemy> enemigos, int dano)
    {
        if (enemigos == null || enemigos.Count == 0)
        {
            Debug.Log("No hay enemigos a los que aplicar dano en area.");
            return;
        }

        for (int i = enemigos.Count - 1; i >= 0; i--)
        {
            Enemy enemigo = enemigos[i];
            enemigo.RecibirDanio(dano);

            Debug.Log($"{enemigo.Nombre} recibe dano en area. Vida restante: {enemigo.Vida}.");

            if (!enemigo.EstaVivo())
            {
                enemigo.Die();
                enemigos.RemoveAt(i);
            }
        }
    }

    public static int EvaluarAtaque(
        int danoBase,
        int defensa,
        float probabilidadCritico,
        ref int vidaActual,
        out bool esCritico)
    {
        int danoFinal = Mathf.Max(0, danoBase - defensa);
        esCritico = Random.Range(0f, 100f) < probabilidadCritico;

        if (esCritico)
        {
            danoFinal *= 2;
        }

        vidaActual = Mathf.Max(0, vidaActual - danoFinal);
        return danoFinal;
    }

    private void EjecutarEjemplo()
    {
        List<Enemy> enemigos = new List<Enemy>
        {
            new Enemy("Goblin", 100, 15, 4),
            new FireEnemy("Salamandra", 120, 12, 5, 8),
            new Enemy("Orco", 80, 20, 2)
        };

        Debug.Log("=== Ejercicio 3: Simulacion de ataque ===");
        SimularAtaque(enemigos);

        Debug.Log("=== Ejercicio 4: EvaluarAtaque ===");
        int vidaCalculada = enemigos[0].Vida;
        bool esCritico;
        int danoAplicado = EvaluarAtaque(
            25,
            enemigos[0].Defensa,
            probabilidadCriticoEjemplo,
            ref vidaCalculada,
            out esCritico);

        Debug.Log(
            $"{enemigos[0].Nombre} recibe {danoAplicado} de dano. Critico: {esCritico}. Vida resultante: {vidaCalculada}.");

        Debug.Log("=== Ejercicio 5: Dano en area ===");
        AplicarDanioEnArea(enemigos, 40);
        Debug.Log($"Enemigos restantes tras el dano en area: {enemigos.Count}.");
    }
}

public class Enemy
{
    public string Nombre { get; private set; }
    public int Vida { get; protected set; }
    public int Ataque { get; private set; }
    public int Defensa { get; private set; }

    public Enemy(string nombre, int vida, int ataque, int defensa)
    {
        Nombre = string.IsNullOrWhiteSpace(nombre) ? "Enemy" : nombre;
        Vida = Mathf.Max(0, vida);
        Ataque = Mathf.Max(0, ataque);
        Defensa = Mathf.Max(0, defensa);
    }

    public void RecibirDanio(int cantidad)
    {
        int danoFinal = Mathf.Max(0, cantidad - Defensa);
        Vida = Mathf.Max(0, Vida - danoFinal);
    }

    public virtual float Atacar()
    {
        return Ataque;
    }

    public bool EstaVivo()
    {
        return Vida > 0;
    }

    public virtual void Die()
    {
        Debug.Log($"{Nombre} ha muerto.");
    }
}

public class FireEnemy : Enemy
{
    public int DanioFuegoExtra { get; private set; }

    public FireEnemy(string nombre, int vida, int ataque, int defensa, int danioFuegoExtra)
        : base(nombre, vida, ataque, defensa)
    {
        DanioFuegoExtra = Mathf.Max(0, danioFuegoExtra);
    }

    public override float Atacar()
    {
        return base.Atacar() + DanioFuegoExtra;
    }

    public override void Die()
    {
        Debug.Log($"{Nombre} se apaga entre llamas.");
    }
}
