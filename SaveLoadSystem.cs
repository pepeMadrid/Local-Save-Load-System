using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SaveLoadSystem : MonoBehaviour {

    public GameObject jugador,dragon;
    public GameObject camara;
    public GameObject camaraInicio;
    public GameObject textos;
    private GameObject jugadorInstanciado;
    void Awake()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu"://si es la primera ejecucion creamos los archivos
                for (int x = 0; x <= 3; x++)
                {
                    crearSlot("SaveSlot" + x);
                }
                crearSlot("SlotAux");
                cargarDatosEnInterfaz();
                break;
            case "Cargando":
                Slot aux = cargarSlots("SlotAux");
                SceneManager.LoadSceneAsync(aux.nombreEscena);
                break;
            case "XampLand":
                interfazTexto = transform.Find("Historia").Find("Texto").GetComponent<TextMeshProUGUI>();
                instaciarJugador();
                cargarDatosEnInterfaz();
                cargarDatosInGame();
                texto = "Desde hace más de 2000 años en la península de los olivos existe una leyenda, una hechicera aparece para combatir la tiranía y la sumisión de los pueblos, unas especies se imponen sobre otras, abusando e incluso exterminando las que no tengan valor, ese valor depende de los intereses de los caudillos, algunos de estos habitantes conocen la leyenda y protegen el lugar donde creen que esta hechicera renace, los Xampis intentan llevar una vida lo más cómoda posible, unos trabajan en las minas para pagar impuestos a los tiranos, otros son granjeros y agricultores.";
                StartCoroutine(mostrarTextoAnimado());
                break;
        }
    }

    private void instaciarJugador()
    {
        Slot aux1 = cargarSlots("SlotAux");
        Vector3 v = new Vector3(aux1.x, aux1.y, aux1.z);
        jugadorInstanciado = Instantiate(jugador, v, Quaternion.Euler(0, 0, 0));
        jugadorInstanciado.GetComponent<MoverPorCLicPC>().interfaz = this.gameObject;
        Instantiate(dragon, jugadorInstanciado.transform.root.Find("PosicionesCamara").Find("DragonPosicion").position, Quaternion.Euler(0, 0, 0));

    }


    public void abrirSlotsGuardar(bool b)
    {
        if (b) { 
        transform.Find("Menu").gameObject.SetActive(false);
        transform.Find("Slots").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Menu").gameObject.SetActive(true);
            transform.Find("Slots").gameObject.SetActive(false);
        }
    }


    public void abrirSlots()
    {
        transform.Find("Menu").gameObject.SetActive(false);
        transform.Find("Slots").gameObject.SetActive(true);
    }

    public void cerrarSlots()
    {

        transform.Find("Menu").gameObject.SetActive(true);
        transform.Find("Slots").gameObject.SetActive(false);
    }


    private void cargarDatosInGame()
    {
        Slot aux = cargarSlots("SlotAux");
        transform.Find("Menu").Find("FondoVida").Find("VidaText").GetComponent<TextMeshProUGUI>().text = aux.vida + "";
        transform.Find("Menu").Find("FondoMana").Find("ManaText").GetComponent<TextMeshProUGUI>().text = aux.mana + "";
        transform.Find("Menu").Find("FondoOro").Find("OroText").GetComponent<TextMeshProUGUI>().text = aux.oro + "";
        for (int a = 1; a <= 5; a++)
        {
            transform.Find("Misiones").Find("Mision" + a).Find("Text").GetComponent<TextMeshProUGUI>().text = aux.misiones[a - 1].titulo;
            transform.Find("Misiones").Find("Mision" + (a - paginaActual * 5)).gameObject.SetActive(aux.misiones[a - 1].activa);
        }
    }

    private void cargarDatosEnInterfaz()
    {
        
        for (int x = 0; x <= 3; x++)
        {
            Slot aux = cargarSlots("SaveSlot" + x);
            transform.Find("Slots").Find("SaveSlot" + x).Find("Fondo").Find("fuego").GetComponent<TextMeshProUGUI>().text = aux.fuego + "";
            transform.Find("Slots").Find("SaveSlot" + x).Find("Fondo").Find("agua").GetComponent<TextMeshProUGUI>().text = aux.agua + "";
            transform.Find("Slots").Find("SaveSlot" + x).Find("Fondo").Find("tierra").GetComponent<TextMeshProUGUI>().text = aux.tierra + "";
            transform.Find("Slots").Find("SaveSlot" + x).Find("Fondo").Find("viento").GetComponent<TextMeshProUGUI>().text = aux.aire + "";
            transform.Find("Slots").Find("SaveSlot" + x).Find("Fondo").Find("EscenaText").GetComponent<TextMeshProUGUI>().text = aux.nombreEscena + "";
            transform.Find("Slots").Find("SaveSlot" + x).Find("Fondo").Find("Vida").Find("Text").GetComponent<TextMeshProUGUI>().text = aux.vida + "";
            transform.Find("Slots").Find("SaveSlot" + x).Find("Fondo").Find("Mana").Find("Text").GetComponent<TextMeshProUGUI>().text = aux.mana + "";
            transform.Find("Slots").Find("SaveSlot" + x).Find("Fondo").Find("Oro").Find("Text").GetComponent<TextMeshProUGUI>().text = aux.oro + "";
        }
    }

    public void seleccionarSlotGuardar(int x)
    {
        Slot aux = cargarSlots("SlotAux");
        aux.x = jugadorInstanciado.transform.position.x;
        aux.y = jugadorInstanciado.transform.position.y;
        aux.z = jugadorInstanciado.transform.position.z;
        guardarSlot("SaveSlot" + x, aux);
        transform.Find("Menu").gameObject.SetActive(true);
        transform.Find("Slots").gameObject.SetActive(false);
        cargarDatosEnInterfaz();

    }
    public void seleccionarSlot(int x)
    {//seleccionamos slot y cargamos la escena "cargando"
        Slot aux = cargarSlots("SaveSlot" + x);
        guardarSlot("SlotAux", aux);
        SceneManager.LoadSceneAsync("Cargando");
    }

    public void cargarEscena(string nombreEscena)
    {
        Slot aux = cargarSlots("SlotAux");
        aux.nombreEscena = nombreEscena;

        switch (nombreEscena)
        {
            case "GoldenTower":
                aux.x = 185;
                aux.y = 0;
                aux.z = 20;
                break;
        }
        
        guardarSlot("SlotAux", aux);
        SceneManager.LoadSceneAsync("Cargando");
    }


    public void completarMision()
    {
        Slot aux = cargarSlots("SlotAux");
        if (!aux.misiones[misionSeleccionada].completado)
            sumarOro(aux.misiones[misionSeleccionada].oro);

        aux = cargarSlots("SlotAux");
        aux.misiones[misionSeleccionada].completado = true;
        guardarSlot("SlotAux", aux);
        jugadorInstanciado.transform.Find("Flecha").gameObject.SetActive(false);
        misionActiva = false;
    }

    private void crearSlot(string nombre)
    {
        if (!File.Exists(Application.persistentDataPath + "/" + nombre + ".GOD"))
        {
            BinaryFormatter bf;
            FileStream file;
            Slot slotNuevo = new Slot();

            bf = new BinaryFormatter();
            file = File.Create(Application.persistentDataPath + "/" + nombre + ".GOD");

            bf.Serialize(file, slotNuevo);
            file.Close();
        }
    }

    private void guardarSlot(string nombre, Slot slot)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + nombre + ".GOD");

        bf.Serialize(file, slot);
        file.Close();
    }

    public Slot cargarSlots(string nombre)
    {
        BinaryFormatter bf;
        FileStream file;
        Slot slotAux = new Slot();

        if (File.Exists(Application.persistentDataPath + "/" + nombre + ".GOD"))
        {
            bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "/" + nombre + ".GOD", FileMode.Open);
            slotAux = (Slot)bf.Deserialize(file);
            file.Close();
        }

        return slotAux;
    }
    [System.Serializable]
    public class Slot
    {
        public int fuego;
        public int agua;
        public int tierra;
        public int aire;

        public int segundosAtaque;
        public int segundosDragon;

        public float x;
        public float y;
        public float z;

        public int vida;
        public int mana;
        public int oro;

        public string nombreEscena;


        public Mision[] misiones;
        public Slot()
        {
            fuego = 1;
            agua = 1;
            tierra = 1;
            aire = 1;

            segundosAtaque = 5;
            segundosDragon = 10;

            x = 375;
            y = 1f;
            z = -70;

            nombreEscena = "Albaida";
            vida = 50;
            mana = 10;
            oro = 0;
            misiones = crearMisiones();

        }
        private Mision[] crearMisiones()
        {
            Mision[] misionesTotales = new Mision[20];

            misionesTotales[0] = new Mision(0,
                "El comienzo",
                "Encuentra a Saturno y empieza tú entrenamiento.",
                10);
            misionesTotales[1] = new Mision(1,
    "Otro guardián",
    "otro guardián de los elementos quiere conocerte, cerca podrás encontrar a Marte",
    15);
            misionesTotales[2] = new Mision(2,
     "El Comienzo",
     "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
     10);
            misionesTotales[3] = new Mision(3,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[4] = new Mision(4,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[5] = new Mision(5,
     "El Comienzo",
     "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
     10);
            misionesTotales[6] = new Mision(6,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[7] = new Mision(7,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[8] = new Mision(8,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[9] = new Mision(9,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[10] = new Mision(10,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[11] = new Mision(11,
     "El Comienzo",
     "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
     10);
            misionesTotales[12] = new Mision(12,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[13] = new Mision(13,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[14] = new Mision(14,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[15] = new Mision(15,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[16] = new Mision(16,
     "El Comienzo",
     "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
     10);
            misionesTotales[17] = new Mision(17,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[18] = new Mision(18,
    "El Comienzo",
    "Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
    10);
            misionesTotales[19] = new Mision(19,
"El Comienzo",
"Adepta tú aprendizaje esta por empezar, escucha la llamada de los elementos, .",
10);
            return misionesTotales;
        }
    }

    //private Mision[] misionesAceptadas = new Mision[5];

    [System.Serializable]
    public class Mision
    {
        public int id;
        public string titulo;
        public string descripcion;
        public int oro;
        public bool completado;
        public bool activa;
        public Mision(int idX,  string tituloX, string descripcionX, int oroX)
        {
            id = idX;
            descripcion = descripcionX;
            oro = oroX;
            completado = false;
            activa = false;
            titulo = tituloX;
        }
    }

}
