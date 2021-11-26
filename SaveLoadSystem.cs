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
            case "Altar":
                instaciarJugador();
                cargarDatosEnInterfaz();
                cargarDatosInGame();
                break;
            case "XampLand":
                interfazTexto = transform.Find("Historia").Find("Texto").GetComponent<TextMeshProUGUI>();
                instaciarJugador();
                cargarDatosEnInterfaz();
                cargarDatosInGame();
                texto = "Desde hace más de 2000 años en la península de los olivos existe una leyenda, una hechicera aparece para combatir la tiranía y la sumisión de los pueblos, unas especies se imponen sobre otras, abusando e incluso exterminando las que no tengan valor, ese valor depende de los intereses de los caudillos, algunos de estos habitantes conocen la leyenda y protegen el lugar donde creen que esta hechicera renace, los Xampis intentan llevar una vida lo más cómoda posible, unos trabajan en las minas para pagar impuestos a los tiranos, otros son granjeros y agricultores.";
                StartCoroutine(mostrarTextoAnimado());
                break;
            case "GoldenTower":
                instaciarJugador();
                cargarDatosEnInterfaz();
                cargarDatosInGame();
                iniciarMazmorra();
                break;
            case "Albaida":
                interfazTexto = transform.Find("Historia").Find("Texto").GetComponent<TextMeshProUGUI>();
                instaciarJugador();
                cargarDatosEnInterfaz();
                cargarDatosInGame();
                //texto = "Desde hace más de 2000 años en la península de los olivos existe una leyenda, una hechicera aparece para combatir la tiranía y la sumisión de los pueblos, unas especies se imponen sobre otras, abusando e incluso exterminando las que no tengan valor, ese valor depende de los intereses de los caudillos, algunos de estos habitantes conocen la leyenda y protegen el lugar donde creen que esta hechicera renace, los Xampis intentan llevar una vida lo más cómoda posible, unos trabajan en las minas para pagar impuestos a los tiranos, otros son granjeros y agricultores.";
                //StartCoroutine(mostrarTextoAnimado());
                break;
            case "PalacioH":
                //interfazTexto = transform.Find("Historia").Find("Texto").GetComponent<TextMeshProUGUI>();
                instaciarJugador();
                cargarDatosEnInterfaz();
                cargarDatosInGame();
                //texto = "Desde hace más de 2000 años en la península de los olivos existe una leyenda, una hechicera aparece para combatir la tiranía y la sumisión de los pueblos, unas especies se imponen sobre otras, abusando e incluso exterminando las que no tengan valor, ese valor depende de los intereses de los caudillos, algunos de estos habitantes conocen la leyenda y protegen el lugar donde creen que esta hechicera renace, los Xampis intentan llevar una vida lo más cómoda posible, unos trabajan en las minas para pagar impuestos a los tiranos, otros son granjeros y agricultores.";
                //StartCoroutine(mostrarTextoAnimado());
                break;
        }
    }


    private string texto;
    private TextMeshProUGUI interfazTexto;
    private int letra = 0;
    private bool mostrado = false;
    public IEnumerator mostrarTextoAnimado()
    {
        while (letra < texto.Length && !mostrado)
        {
            interfazTexto.text += texto[letra];
            yield return new WaitForSeconds(0.05f);
            letra++;
        }

        if (letra == texto.Length)
        {
            mostrado = true;
            letra = 0;
        }
    }



    public void activarMenu(bool b)
    {

        transform.Find("Menu").Find("Bot").gameObject.SetActive(b);
        if (b)
        {
            jugadorInstanciado.GetComponent<NavMeshAgent>().SetDestination(jugadorInstanciado.transform.position);
            jugadorInstanciado.GetComponent<MoverPorCLicPC>().destruirParticulas();
            transform.Find("Menu").Find("AbrirMenu").gameObject.SetActive(false);
            jugadorInstanciado.GetComponent<MoverPorCLicPC>().enabled = false;
        }
        else
        {
            transform.Find("Menu").Find("AbrirMenu").gameObject.SetActive(true);
            jugadorInstanciado.GetComponent<MoverPorCLicPC>().enabled = true;
        }

    }

    public void camAdelante()
    {
        jugadorInstanciado.transform.Find("VCam").transform.Translate(Vector3.forward * 5, Space.Self);
    }
    public void camAtras()
    {
        jugadorInstanciado.transform.Find("VCam").transform.Translate(-Vector3.forward * 5, Space.Self);
    }
    public void camIzquierda()
    {
        jugadorInstanciado.transform.Find("VCam").transform.Translate(-Vector3.right * 5, Space.Self);
    }
    public void camDerecha()
    {
        jugadorInstanciado.transform.Find("VCam").transform.Translate(Vector3.right * 5, Space.Self);
    }


    private bool ojo = false;
    public void botonOjo()
    {
        if (!ojo)
        {
            transform.Find("Historia").Find("Texto").gameObject.SetActive(false);
            ojo = true;
        }
        else
        {
            transform.Find("Historia").Find("Texto").gameObject.SetActive(true);
            ojo = false;
        }
    }
    public void botonTienda(bool b)
    {
        if (b)
        {
            Slot aux = cargarSlots("SlotAux");
            transform.Find("Menu").gameObject.SetActive(false);
            transform.Find("Tienda").gameObject.SetActive(true);
            transform.Find("Tienda").Find("FondoOro").Find("OroText").GetComponent<TextMeshProUGUI>().text = aux.oro + "";
            transform.Find("Tienda").Find("FondoMana").Find("ManaText").GetComponent<TextMeshProUGUI>().text = aux.mana + "";
            transform.Find("Tienda").Find("FondoVida").Find("VidaText").GetComponent<TextMeshProUGUI>().text = aux.vida + "";
        }
        else
        {
            transform.Find("Menu").gameObject.SetActive(true);
            transform.Find("Tienda").gameObject.SetActive(false);
        }

    }
    public void botonAceptar()
    {
        if (!mostrado)
        {
            interfazTexto.text = texto;
            mostrado = true;

        }
        else
        {
            Destroy(camaraInicio);
            Destroy(textos);
            jugadorInstanciado.GetComponent<MoverPorCLicPC>().enabled = true;
            jugadorInstanciado.GetComponent<EleccionesEvento>().enabled = true;
            jugadorInstanciado.transform.Find("VCam").Find("Camara").GetComponent<Camera>().enabled = true;
            jugadorInstanciado.transform.Find("VCam").Find("Camara").GetComponent<AudioListener>().enabled = true;
            transform.Find("Historia").gameObject.SetActive(false);
            transform.Find("Menu").gameObject.SetActive(true);
        }

    }
    private void iniciarMazmorra()
    {
        jugadorInstanciado.GetComponent<MoverPorCLicPC>().enabled = true;
        jugadorInstanciado.transform.Find("VCam").Find("Camara").GetComponent<Camera>().enabled = true;
        jugadorInstanciado.transform.Find("VCam").Find("Camara").GetComponent<AudioListener>().enabled = true;
        transform.Find("Historia").gameObject.SetActive(false);
        transform.Find("Menu").gameObject.SetActive(true);

    }

    public void botonMas()
    {
        if (camaraInicio.GetComponent<VolarPorPuntosCamara>().velocidad < 30)
            camaraInicio.GetComponent<VolarPorPuntosCamara>().velocidad += 5;
    }
    public void botonMenos()
    {
        if (camaraInicio.GetComponent<VolarPorPuntosCamara>().velocidad > 0)
            camaraInicio.GetComponent<VolarPorPuntosCamara>().velocidad -= 5;
    }

    private int costeTeleport = 0;
    public bool restarMana()
    {
        Slot aux = cargarSlots("SlotAux");
        if (aux.mana >= costeTeleport)
        {
            aux.mana -= costeTeleport;
            guardarSlot("SlotAux", aux);
            cargarDatosInGame();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void compraVida()
    {
        print("XXXXX");
        Slot aux = cargarSlots("SlotAux");
        if (aux.oro >= 5){
        
            aux.vida += 10;
            aux.oro -= 5;
            guardarSlot("SlotAux", aux);
            cargarDatosInGame();
            botonTienda(false);
        }
    }
    public void compraVidaX5()
    {
        Slot aux = cargarSlots("SlotAux");
        if (aux.oro >= 25) {

            aux.vida += 50;
            aux.oro -= 25;
            guardarSlot("SlotAux", aux);
            cargarDatosInGame();
            botonTienda(false);
        }
    }
    public void compraMana()
    {
        print("XXXXX");
        Slot aux = cargarSlots("SlotAux");
        if (aux.oro >= 10) { 
            aux.mana += 10;
            aux.oro -= 10;
            guardarSlot("SlotAux", aux);
            cargarDatosInGame();
            botonTienda(false);
        }
    }
    public void compraManaX5()
    {
        Slot aux = cargarSlots("SlotAux");
        if (aux.oro >= 50) {
       
            aux.mana += 50;
            aux.oro -= 50;
            guardarSlot("SlotAux", aux);
            cargarDatosInGame();
            botonTienda(false);
        }
    }

    public void vidaFairy(int n)
    {

        Slot aux = cargarSlots("SlotAux");
        aux.vida += n;
        guardarSlot("SlotAux", aux);
        cargarDatosInGame();

    }
    public void sumarOro(int n)
    {

        Slot aux = cargarSlots("SlotAux");
        aux.oro += n;
        guardarSlot("SlotAux", aux);
        cargarDatosInGame();

    }
    public void restarVida(int n, int x)
    {
        StartCoroutine(animacionDamage());
        Slot aux = cargarSlots("SlotAux");
        aux.vida -= Random.Range(n, x);
        guardarSlot("SlotAux", aux);
        cargarDatosInGame();
    }
    IEnumerator animacionDamage()
    {
        jugadorInstanciado.transform.Find("Bruja").GetComponent<Animator>().SetBool("Damage", true);
        yield return new WaitForSeconds(0.25f);
        jugadorInstanciado.transform.Find("Bruja").GetComponent<Animator>().SetBool("Damage", false);
    }
    public void manaFairy(int n)
    {
        Slot aux = cargarSlots("SlotAux");

        aux.mana += n;
        guardarSlot("SlotAux", aux);
        cargarDatosInGame();

    }

    private void instaciarJugador()
    {
        Slot aux1 = cargarSlots("SlotAux");
        Vector3 v = new Vector3(aux1.x, aux1.y, aux1.z);
        jugadorInstanciado = Instantiate(jugador, v, Quaternion.Euler(0, 0, 0));
        jugadorInstanciado.GetComponent<MoverPorCLicPC>().interfaz = this.gameObject;
        Instantiate(dragon, jugadorInstanciado.transform.root.Find("PosicionesCamara").Find("DragonPosicion").position, Quaternion.Euler(0, 0, 0));

    }


    public void activarCamara(bool b)
    {
        transform.Find("Camara").gameObject.SetActive(b);
        if (b)
        {
            transform.Find("Menu").gameObject.SetActive(false);
            //jugadorInstanciado.transform.Find("VCam").Find("Camara").position = 
        }
        else
        {
            transform.Find("Menu").gameObject.SetActive(true);
            //jugadorInstanciado.transform.Find("VCam").Find("Camara").GetComponent<LookCamara>().activo = false;
        }

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
    public void abrirQuest(int chapter, int mision)
    {
        Slot aux = cargarSlots("SlotAux");
        aux.misiones[mision - 1].activa = true;
        print("Chapter [" + chapter + "] Quest [" + mision + "]");
        transform.Find("Quest").Find("Titulo").GetComponent<TextMeshProUGUI>().text = "Chapter ["+chapter +"] Quest ["+mision+"]";
        transform.Find("Quest").Find("Texto").GetComponent<TextMeshProUGUI>().text = aux.misiones[mision - 1].descripcion;
        transform.Find("Menu").gameObject.SetActive(false);
        transform.Find("Quest").gameObject.SetActive(true);
        jugadorInstanciado.GetComponent<MoverPorCLicPC>().enabled = false;
        guardarSlot("SlotAux", aux);


    }

    public bool comprobarQuestActiva(int mision)
    {
        Slot aux = cargarSlots("SlotAux");
        return aux.misiones[mision - 1].activa;

    }
    public void cerrarQuest()
    {
        transform.Find("Menu").gameObject.SetActive(true);
        transform.Find("Quest").gameObject.SetActive(false);
        jugadorInstanciado.GetComponent<MoverPorCLicPC>().enabled = true;
    }

        public void abrirMisiones(bool b)
    {
        if (b) { 
            transform.Find("Menu").gameObject.SetActive(false);
            transform.Find("Misiones").gameObject.SetActive(true);
            cargarDatosInGame();
        }
        else
        {
            transform.Find("Menu").gameObject.SetActive(true);
            transform.Find("Misiones").gameObject.SetActive(false);
            transform.Find("Misiones").Find("Seleccionada").gameObject.SetActive(false);
        }
    }

    public void seleccionarMision(bool b)
    {
        if (b)
        {
            jugadorInstanciado.transform.Find("Flecha").gameObject.SetActive(true);
            Slot aux = cargarSlots("SlotAux");
            jugadorInstanciado.transform.Find("Flecha").GetComponent<LookCamaraVector3>().objetivo = new Vector3(GetComponent<QuestPool>().misiones[misionSeleccionada].transform.position.x, GetComponent<QuestPool>().misiones[misionSeleccionada].transform.position.y, GetComponent<QuestPool>().misiones[misionSeleccionada].transform.position.z);
            jugadorInstanciado.transform.Find("Flecha").GetComponent<LookCamaraVector3>().activo = true;
            transform.Find("Misiones").Find("Seleccionada").gameObject.SetActive(false);
            abrirMisiones(false);
            GetComponent<QuestPool>().misiones[misionSeleccionada].SetActive(true);
            misionActiva = true;
            activarMenu(false);
        }
        else
        {
            jugadorInstanciado.transform.Find("Flecha").gameObject.SetActive(false);
            jugadorInstanciado.transform.Find("Flecha").GetComponent<LookCamaraVector3>().activo = false;
            transform.Find("Misiones").Find("Seleccionada").gameObject.SetActive(false);
            abrirMisiones(false);
            GetComponent<QuestPool>().misiones[misionSeleccionada].SetActive(false);
            misionActiva = false;
            activarMenu(false);

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

    private int paginaActual = 0;
    public void pagina(bool b) {
        Slot aux = cargarSlots("SlotAux");
        bool active = false;
        if (b && paginaActual< aux.misiones.Length/5 - 1)
        {
            paginaActual++;
            active = true;

        }
        else if (!b && paginaActual > 0)
        {
            paginaActual--;
            active = true;
        }
        if (active)
        {
            transform.Find("Misiones").Find("Text").GetComponent<TextMeshProUGUI>().text = "Chapter " + (paginaActual + 1);
            for (int a = 1 + paginaActual * 5; a <= 5 + paginaActual * 5; a++)
            {

                transform.Find("Misiones").Find("Mision" + (a - paginaActual * 5)).Find("Text").GetComponent<TextMeshProUGUI>().text = aux.misiones[a - 1].titulo;
                transform.Find("Misiones").Find("Mision" + (a - paginaActual * 5)).gameObject.SetActive(aux.misiones[a - 1].activa);
            }
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
    private int misionSeleccionada = 0;
    public bool misionActiva = false;
    public void botonMision(int x)
    {
        misionSeleccionada = x + paginaActual * 5;
        Slot aux = cargarSlots("SlotAux");
        transform.Find("Misiones").Find("Seleccionada").Find("Titulo").GetComponent<TextMeshProUGUI>().text = aux.misiones[misionSeleccionada].titulo +"";
        transform.Find("Misiones").Find("Seleccionada").Find("Descripcion").GetComponent<TextMeshProUGUI>().text = aux.misiones[misionSeleccionada].descripcion + "";
        transform.Find("Misiones").Find("Seleccionada").Find("Oro").GetComponent<TextMeshProUGUI>().text = aux.misiones[misionSeleccionada].oro + "";
        transform.Find("Misiones").Find("Seleccionada").gameObject.SetActive(true);
        if (aux.misiones[misionSeleccionada].completado)
            transform.Find("Misiones").Find("Seleccionada").Find("Oro").Find("Completada").gameObject.SetActive(true);
        else
            transform.Find("Misiones").Find("Seleccionada").Find("Oro").Find("Completada").gameObject.SetActive(false);

    }
    public void seleccionarQuit()
    {
        SceneManager.LoadScene("MainMenu");
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
    {
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
