# Shooting Range in Unity - VR user interactie en Machine Learning object detectie

## Inleiding

In deze tutorial behandelen we hoe VR en machine learning kunnen toegepast worden binnen Unity om een goede gebruikservaring van onze game te verkrijgen. Machinelearning en raycasting is een belangrijke techniek voor moderne games. Hiermee kunnen er zeer uitdagende games ontwikkeld worden die gamers aanspreken. We leggen uit hoe we deze raycasting, machine learning en VR kunnen implementeren in je eigen projecten.

## Samenvatting

Na het doorlopen van deze tutorial weet je hoe je raycasting, machine learning en VR kan configureren en toepassen in je eigen projecten, met behulp van de cursus "VR Experience". Je bent in staat om dit tutorial project zelfstandig opnieuw op te bouwen en eventueel uitbreiden. Je krijgt inzicht over hoe raycasting kan gebruikt worden binnen een ml-agent, hoe een ml-agent geïntegreerd kan worden in een VR applicatie en hoe je een interactieve VR-game kunt ontwikkelen.

## Methoden

### Versies

- Unity: 6000.0.36f1 LTS
- ML-Agents Unity plugin: 3.0.0
- ML-agents Python: 0.30.0
- Python: 3.9.21
- TensorBoard: 2.8.0
- TensorFlow: 2.8.0
- OpenXR Plugin: 1.14.0
- XR Hands: 1.5.0
- XR Interaction Toolkit: 3.0.7
- XR Plugin Management: 4.5.0

### Verloop

Bij de opstart van het spel moet de speler het spel handmatig starten. Vervolgens krijgt de speler een beperkte tijd om zo veel mogelijk doelwitten te raken in de shooting range. De goal van het spel is voor de speler om een hogere score te behalen dan de ML-agent.

Er wordt altijd één doelwit gegenereerd dat voor beiden de ML-agent en speler dient. Als de speler het doelwit mist, krijgt hij/zij geen straf, maar zal de ML-agent kan alsnog wel dit doelwit raken en zo de scoren stelen. wanneer de speler het doelwit wel raakt, ontvangt hij/zij een score afhankelijk van de kleur van het geraakte deel van het doelwit.

Eens het doelwit geraakt is door de ML-agent of speler, verdwijnt het en wordt er automatisch een nieuw doelwit geplaatst. Het spel loopt door tot de bepaalde tijd verstreken is.

### ML-agent beloningen, acties en observaties

Voor de ML-agent is het belangrijk de juiste keuzes te maken. Zo worden ze opgesplitst in typisch een 3 tal keuzes.

#### Observaties

Één van deze belangrijke keuzes is beslissen wat de agent allemaal kan zien en wat hij allemaal moet leren. Zo zijn we er bij dit project voor gegaan dat de agent het target zijn locatie weet staan om de training sneller en makkelijker te laten verlopen. Ook zullen we de agent zijn eigen rotatie als ook de "rechtdoor" richting van het geweer meegeven zodat hij altijd weet waar deze zich bevind. In onze prefab van het geweer dat gebruikt is, is de voorkant van het geweer echter:

``` C#
  sensor.AddObservation((-aiGun.transform.right).normalized);
```

Dit komt door de keuzes van de originele developer van deze asset.

#### Acties en beloningen

De agent zijn acties bestaan uit slechts 2 componenten, een continuous actie voor het roteren en een continuous actie voor het schieten. In onze "OnActionReceived" kunnen we dan volgende bewerkingen doen:

```C#
public override void OnActionReceived(ActionBuffers actions)
{
  float horizontal = actions.ContinuousActions[0];
  float shoot = actions.ContinuousActions[1];

  Debug.Log($"shoot: {shoot}, horizontal: {horizontal}");
  transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);

  SetReward(-0.001f);

  if (shoot >= 0.5f && currentShotCount < maxShots)
  {
    Debug.Log("Shooting");
    currentShotCount++;
    SetReward(0.001f);
    Debug.DrawRay(aiGun.transform.position, -aiGun.transform.right*fovDistance, Color.red);
    animator.SetTrigger("Shoot");
    if (Physics.Raycast(aiGun.transform.position, -aiGun.transform.right, out RaycastHit hit, fovDistance) && hit.transform.gameObject.layer == 6)
    {
      Debug.Log("Hit");
      SetReward(2.0f);
      ScoreScript.AddScoreML(hit.transform.gameObject.GetComponent<TargetScript>().Hit());
      EndEpisode();
    }
  }
  else if (shoot < 0.5f && currentShotCount < maxShots)
  {
    SetReward(-0.01f);
  }
  else
  {
    SetReward(-1.0f);
    EndEpisode();
  }
}
```

Eerst worden de acties gelezen om deze verder te gebruiken in het programma. Zo zal de "horizontal" actie al onmiddellijk gebruikt worden voor het roteren van de agent. Bij elke rotatie krijgt de agent een minimale straf van 0.001 om het willekeurig en veel roteren te ontmoedigen. Vervolgens wordt de "shoot" actie onder handen genomen. Als de "shoot" variabele groter is dan 0.5 zullen we dit als echt schieten zien. Echter mag de agent maar een beperkt aantal keer schieten voor de episode eindigt. Zolang dit maximum niet bereikt is zullen we de "shoot" variable van hoger dan 0.5 ook echt laten schieten. Het aantal schoten word verhoogt en een debug lijn wordt getekend (dit maakt het gemakkelijker om de agent in het oog te houden). We zullen ook de animatie van onze animatiecontroller een trigger voor het schieten sturen, later hier meer over. Het schieten krijgt een zere kleine beloning voor het voorkomen dat de agent niet meer wilt schieten. Tenslotte zullen we controleren of een schot van het geweer wel degelijk het target raakt. Indien het geraakt word zullen we een grote beloning geven, het target vernietigen, de score optellen bij het globaal en de episode eindigen.

Echter als de waarde van "shoot" kleiner is dan 0.5 zal dit als vals schot gezien worden. Tenslotte is er ook een clause voor de laatste optie, namelijk als currentShotCount groter is dan maxShots, dan zal de episode eindigen en als mislukt worden beschouwd met een straf van -1.0.

#### Beloningen extra

In de toekomst kan deze implementatie ook nog verbeterd worden door de beloningen nog beter te beheren. Zo kunnen we bijvoorbeeld de beloning wanneer "shoot" kleiner is dan 0.5 veranderen aangezien deze een beetje tegenstrijdig is.

### Objecten

#### ML-agent

Zoals hiervoor besproken heeft de ML-Agent de kennis van de target nodig om goed te functioneren. Daarom zullen we een extra functie toevoegen aan het agentScript zodat we een nieuwe target kunnen toewijzen via een extern script.

```C#
  public void SetNewTarget(GameObject target)
  {
    this.target = target;
  }
```

Vervolgens gaan we ook de agent zijn standaard rotatie laten herinneren en resetten wanneer de episode eindigt.

```C#
void Start()
{
  // Get the Animator from a child GameObject
  animator = GetComponentInChildren<Animator>();
  Transform deagle = transform.Find("Deagle"); // get gun to make invisable
  if (deagle != null)
  {
    Renderer deagleRenderer = deagle.GetComponent<Renderer>();
    if (deagleRenderer != null)
    {
      deagleRenderer.enabled = false;  // Makes Deagle invisible
    }
  }
}

public override void OnEpisodeBegin()
{
  this.transform.localRotation = Quaternion.Euler(0, -90, 0);
  this.currentShotCount = 0;
}
```

Dit zorgt voor een snellere en vlottere training, maar als je nog een meer zelf lerende ai wil maken moet je dit niet doen, net zoals de target locatie.

In de start functie hebben we er ook voor gezorgd dat de gun prefab dat we gebruiken voor de player en de logica onzichtbaar maken maar wel actief houden. We hebben er namelijk voor gekozen om een asset te gebruiken om de agent een leuke skin te geven. Echter om bij de gratis assets te blijven vonden we de "Robot Hero : PBR HP Polyart" het beste passen. Deze heeft een eigen geweer prefab waar al animaties voor beschikbaar zijn, vandaar onze keuze. De logica is nog altijd gebaseerd op de deagle prefab.

Nu we bij het bespreken van deze asset gekomen zijn, kunnen we hier dieper op in gaan. De asset zijn standaard animatie controller in niet zoals het moet voor onze situatie dus zullen we deze aanpassen zodat enkel de volgende 3 animaties overblijven:

IMAGE

Door aan de overgang van de idle naar de shoot animatie een trigger van "Shoot" te bevestigen hebben we deze mooi geïmplementeerd in de ML agent. Nu kunnen we het agent script aan het agent object (empty game object) hangen en de deagle prefab alsook de PBRCharacter prefab (van de Robot Hero : PBR HP Polyart asset) als children bevestigen aan de agent.

IMAGE

#### Target-Controller

#### Target

#### Shooting range

#### Gun

### Gedrag objecten

### One-pager

### Afwijkingen one-pager

## Resultaten

### Training

### Waarnemingen

## Conclusie

## Bronnen

### Notities

> Sommige bronnen zijn geraadpleegd voor begrip en ontwikkeling tijdens het project, ook al is de inhoud ervan niet zichtbaar in het eindresultaat. Alle bronnen zijn opgenomen voor transparantie en volledigheid.

> APA citaties in dit document zijn gegenereerd en gevormd met behulp van CHATGPT (OpenAI, 2025). De bronnen zijn door ons gekozen.

> Dit document is ook opgesteld met behulp van CHATGPT (OpenAI, 2025) als begeleidende factor voor formulering van zinnen en tekst alsook spellingscorrectie.

> Sommige stukken code en inzichten zijn gebaseerd op oefeningen en richtlijnen uit de cursussen "VR Experience" en "3D Game Programming" van Tom Peeters, gegeven aan AP Hogeschool in 2025 en 2023 respectievelijk.

### Bronvermelding

- GeeksforGeeks. (2020, October 15). C# break statement. <https://www.geeksforgeeks.org/c-sharp-break-statement/>
- Microsoft Copilot. (n.d.). copilot.microsoft.com. <https://copilot.microsoft.com> (Geraadpleegd op 12 mei 2025; geen code overgenomen in eindversie)
- OpenAI. (2025). ChatGPT (June 5 version) [gpt-4-1-mini]. Gebruikt voor generatie van APA 7 citaties. <https://chat.openai.com/>
- Stack Overflow. (2015, February 15). Raycast to get GameObject being hit to run script and function on GameObject [Answer]. Stack Overflow. <https://stackoverflow.com/questions/30020429/raycast-to-get-gameobject-being-hit-to-run-script-and-function-on-gameobject>
- Unity. (2025, June 4). Debug.DrawLine. <https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html>
- Unity. (2025, June 4). Debug.DrawLine (v5.3.0). <https://docs.unity3d.com/530/Documentation/ScriptReference/Debug.DrawLine.html>
- Unity. (2025, June 4). GameObject.tag. Unity. <https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject-tag.html>
- Unity. (2025, June 4). Unity.MLAgents.Sensors.RayPerceptionOutput. <https://docs.unity3d.com/Packages/com.unity.ml-agents@1.0/api/Unity.MLAgents.Sensors.RayPerceptionOutput.html>
- Unity. (2025, June 4). Vector3.Angle. <https://docs.unity3d.com/ScriptReference/Vector3.Angle.html>
- Unity Masters Hub. (2023). Implementing raycasting in Unity 3D: A step-by-step guide. <https://unitymastershub.com/implementing-raycasting-in-unity-3d-a-step-by-step-guide-2/>
- Unity Technologies. (2015, March 5). Making a Raycast visible [Online forum post]. Unity Forums. <https://discussions.unity.com/t/making-a-raycast-visible/135103>
- Unity Technologies. (2019, January 10). What does really mean the forward direction? [Online forum post]. Unity Forums. <https://discussions.unity.com/t/what-does-really-mean-the-forward-direction/842678>
- Unity Technologies. (2019, February 15). Why when using Vector3.Dot the dot value is never more than 1? [Online forum post]. Unity Forums. <https://discussions.unity.com/t/why-when-using-vector3-dot-the-dot-value-is-never-more-then-1/854406>
- Unity Technologies. (2019, July 19). How do I reset the rotation? [Online forum post]. Unity Forums. <https://discussions.unity.com/t/how-do-i-reset-the-rotation/236760>
- Unity Technologies. (2020, November 13). How to check if Raycast hits a certain object [Online forum post]. Unity Forums. <https://discussions.unity.com/t/how-to-check-if-raycast-hits-a-certain-object/248279>
- Unity Technologies. (2021, September 2). How to check Ray Perception Sensor 3D [Online forum post]. Unity Forums. <https://discussions.unity.com/t/how-to-check-ray-perception-sensor-3d/914089/2>
- Unity Technologies. (2022, May 25). Raycasting for interactables [Online forum post]. Unity Forums. <https://discussions.unity.com/t/raycasting-for-interactables/920097>
- Unity Technologies. (2022, July 25). How to search a list for GameObjects with tag and copy them into a new list? [Online forum post]. Unity Forums. <https://discussions.unity.com/t/how-to-search-a-list-for-gameobjects-with-tag-and-copy-them-into-a-new-list/665075/6>
- Unity Technologies. (2023, April 13). Angle between two 3D points on a plane [Online forum post]. Unity Forums. <https://discussions.unity.com/t/angle-between-two-3d-points-on-a-plane/252685>
- Van Daele, B. (2023). CubeAgent.cs [Source code]. GitHub. <https://github.com/AP-IT-GH/jumper-assignment-BrentVanDaele/blob/main/Jumper/Assets/Scripts/CubeAgent.cs>
- Wikipedia contributors. (2025, March 5). Field of view. Wikipedia. Retrieved June 5, 2025, from <https://en.wikipedia.org/wiki/Field_of_view>
- Peeters, T. (2025). VR Experience [Cursusmateriaal, AP Hogeschool]. Persoonlijke lessen en oefenopdrachten gebruikt als referentie en basis voor implementatie.
- Peeters, T. (2023). 3D Game Programming [Cursusmateriaal, AP Hogeschool]. Gebruikt voor codevoorbeelden, technische inzichten en referentie tijdens projectontwikkeling.
