# Shooting Range in Unity - VR user interactie en Machine Learning object detectie

## Inleiding

In deze tutorial behandelen we hoe VR en machine learning kunnen toegepast worden binnen Unity om een goede gebruikservaring van onze game te verkrijgen. Machinelearning en raycasting is een belangrijke techniek voor moderne games. Hiermee kunnen er zeer uitdagende games ontwikkeld worden die gamers aanspreken. We leggen uit hoe we deze raycasting, machine learning en VR kunnen implementeren in je eigen projecten.

## Samenvatting

Na het doorlopen van deze tutorial weet je hoe je raycasting, machine learning en VR kan configureren en toepassen in je eigen projecten. Je bent in staat om dit tutorial project zelfstandig opnieuw op te bouwen en eventueel uitbreiden. Je krijgt inzicht over hoe raycasting kan gebruikt worden binnen een ml-agent, hoe een ml-agent geïntegreerd kan worden in een VR applicatie en hoe je een interactieve VR-game kunt ontwikkelen.

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

Er9 wordt altijd één doelwit gegenereerd dat voor beiden de ML-agent en speler dient. Als de speler het doelwit mist, krijgt hij/zij geen straf, maar zal de ML-agent kan alsnog wel dit doelwit raken en zo de scoren stelen. wanneer de speler het doelwit wel raakt, ontvangt hij/zij een score afhankelijk van de kleur van het geraakte deel van het doelwit.

Eens het doelwit geraakt is door de ML-agent of speler, verdwijnt het en wordt er automatisch een nieuw doelwit geplaatst. Het spel loopt door tot de bepaalde tijd verstreken is.

### ML-agent beloningen, acties en observaties

### Objecten

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
