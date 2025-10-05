using UnityEngine;
using OpenAI;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

public class ApiIntegration : MonoBehaviour
{
    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    public GameObject theContent;
    // Your dictionary data
    private Dictionary<string, Dictionary<string, string>> objects = new Dictionary<string, Dictionary<string, string>> {
        {"UHF Antenna", new Dictionary<string, string> { {"Price", "200"}, {"Power", "-10"}, {"Cooling", "-1"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "-2"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "-5%"} } },
        {"S-band Antenna", new Dictionary<string, string> { {"Price", "300"}, {"Power", "-15"}, {"Cooling", "-2"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "-2"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "-7%"} } },
        {"Ku-band Antenna", new Dictionary<string, string> { {"Price", "400"}, {"Power", "-20"}, {"Cooling", "-3"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "-3"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "-10%"} } },
        {"Data Hub (DMS)", new Dictionary<string, string> { {"Price", "600"}, {"Power", "-30"}, {"Cooling", "-10"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "3"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "3"}, {"Morale", "0"}, {"SystemRiskMod", "-2"}, {"CommsRiskMod", "-3"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "-10%"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "-40%"} } },
        {"High-Gain Ku-band Antenna", new Dictionary<string, string> { {"Price", "900"}, {"Power", "-40"}, {"Cooling", "-15"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "2"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "-4"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "-15%"} } },
        {"AI Core", new Dictionary<string, string> { {"Price", "1200"}, {"Power", "-60"}, {"Cooling", "-30"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "-3"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "3"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "-40%"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "-20%"}, {"Data Loss (12%)", "0"} } },
        {"Command Workstation", new Dictionary<string, string> { {"Price", "300"}, {"Power", "-10"}, {"Cooling", "-2"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "-2"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "-2"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "-15%"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Navigation System", new Dictionary<string, string> { {"Price", "1000"}, {"Power", "-50"}, {"Cooling", "-40"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "-3"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "-3"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "-30%"}, {"Data Loss (12%)", "0"} } },
        {"ARED Device", new Dictionary<string, string> { {"Price", "700"}, {"Power", "-20"}, {"Cooling", "-8"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "1"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-2"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"CEVIS Cycle", new Dictionary<string, string> { {"Price", "500"}, {"Power", "-15"}, {"Cooling", "-6"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "1"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-2"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"COLBERT Treadmill", new Dictionary<string, string> { {"Price", "600"}, {"Power", "-20"}, {"Cooling", "-8"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "1"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-2"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Upright Sleep Pod", new Dictionary<string, string> { {"Price", "400"}, {"Power", "-5"}, {"Cooling", "-1"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "1"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "2"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-3"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Freeze-Dried Food Packets", new Dictionary<string, string> { {"Price", "150"}, {"Power", "0"}, {"Cooling", "0"}, {"Calories", "1400"}, {"Protein", "80"}, {"Carbs", "400"}, {"Fat", "100"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "3"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-1"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Espresso Machine", new Dictionary<string, string> { {"Price", "300"}, {"Power", "-15"}, {"Cooling", "-4"}, {"Calories", "60"}, {"Protein", "2"}, {"Carbs", "8"}, {"Fat", "1"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "3"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-1"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Thermostabilized Food", new Dictionary<string, string> { {"Price", "200"}, {"Power", "0"}, {"Cooling", "0"}, {"Calories", "1400"}, {"Protein", "60"}, {"Carbs", "200"}, {"Fat", "80"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "3"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "1"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-1"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Food Rehydration Station", new Dictionary<string, string> { {"Price", "250"}, {"Power", "-10"}, {"Cooling", "-4"}, {"Calories", "2000"}, {"Protein", "80"}, {"Carbs", "90"}, {"Fat", "60"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "2"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Fridge", new Dictionary<string, string> { {"Price", "600"}, {"Power", "-20"}, {"Cooling", "-15"}, {"Calories", "4000"}, {"Protein", "100"}, {"Carbs", "250"}, {"Fat", "120"}, {"Water", "-2"}, {"Oxygen", "0"}, {"Waste", "2"}, {"Storage", "0"}, {"Treatments", "1"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "1"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Hydroponics Farm", new Dictionary<string, string> { {"Price", "1200"}, {"Power", "-50"}, {"Cooling", "-30"}, {"Calories", "10000"}, {"Protein", "150"}, {"Carbs", "300"}, {"Fat", "50"}, {"Water", "-3"}, {"Oxygen", "3"}, {"Waste", "-2"}, {"Storage", "2"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "2"}, {"Morale", "4"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "-2"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-3"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Water Dispenser", new Dictionary<string, string> { {"Price", "150"}, {"Power", "-5"}, {"Cooling", "-1"}, {"Calories", "100"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "5"}, {"Oxygen", "2"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "-1"}, {"NavRiskMod", "-1"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "1"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Medical Soft Goods", new Dictionary<string, string> { {"Price", "200"}, {"Power", "0"}, {"Cooling", "0"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "-1"}, {"Storage", "0"}, {"Treatments", "3"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "-15%"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Orcheo Lite TE Ultrasound Scanner", new Dictionary<string, string> { {"Price", "600"}, {"Power", "-15"}, {"Cooling", "-6"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "4"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "-10%"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Defibrillator", new Dictionary<string, string> { {"Price", "150"}, {"Power", "-5"}, {"Cooling", "-1"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "3"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "-80%"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Ambulatory Medical Pack", new Dictionary<string, string> { {"Price", "400"}, {"Power", "-10"}, {"Cooling", "-3"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "2"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "-25%"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Ultrasound System", new Dictionary<string, string> { {"Price", "700"}, {"Power", "-20"}, {"Cooling", "-6"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "4"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "2"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-1"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "-15%"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Battery Bank", new Dictionary<string, string> { {"Price", "800"}, {"Power", "100"}, {"Cooling", "-20"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "-1"}, {"Storage", "2"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "-2"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "-15%"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Fuel Cell Generator", new Dictionary<string, string> { {"Price", "900"}, {"Power", "120"}, {"Cooling", "-40"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "3"}, {"Storage", "4"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "-1"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "-3"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "-20%"}, {"Fire Hazard (4%)", "4%"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Reactor Core", new Dictionary<string, string> { {"Price", "2000"}, {"Power", "400"}, {"Cooling", "-300"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "4"}, {"Storage", "5"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "-2"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "4"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "3"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "-30%"}, {"Fire Hazard (4%)", "4%"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Fusion Reactor", new Dictionary<string, string> { {"Price", "2500"}, {"Power", "500"}, {"Cooling", "-400"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "3"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "5"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "4"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "-30%"}, {"Fire Hazard (4%)", "4%"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Cryo Pump", new Dictionary<string, string> { {"Price", "700"}, {"Power", "-30"}, {"Cooling", "300"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "-3"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "-20%"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Heat Exchanger", new Dictionary<string, string> { {"Price", "600"}, {"Power", "-20"}, {"Cooling", "200"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "-2"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "-20%"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Radiator Panel", new Dictionary<string, string> { {"Price", "1000"}, {"Power", "0"}, {"Cooling", "600"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "-5"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "-40%"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Space Toilet", new Dictionary<string, string> { {"Price", "500"}, {"Power", "-10"}, {"Cooling", "-5"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "2"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "-20%"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Waste Recycler", new Dictionary<string, string> { {"Price", "1000"}, {"Power", "-40"}, {"Cooling", "-40"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "4"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "-40%"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Waste Bioreactor", new Dictionary<string, string> { {"Price", "1200"}, {"Power", "-60"}, {"Cooling", "-60"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "-1"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "-60%"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Oxygen Generation System", new Dictionary<string, string> { {"Price", "1500"}, {"Power", "-80"}, {"Cooling", "-50"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "3"}, {"Oxygen", "2"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Water Electrolyzer", new Dictionary<string, string> { {"Price", "400"}, {"Power", "-15"}, {"Cooling", "-10"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "-2"}, {"Oxygen", "5"}, {"Waste", "5"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "1"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-2"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Advanced Purifier", new Dictionary<string, string> { {"Price", "700"}, {"Power", "-25"}, {"Cooling", "-15"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "4"}, {"Oxygen", "0"}, {"Waste", "4"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "2"}, {"Morale", "0"}, {"SystemRiskMod", "-2"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "-1"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "-20%"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"3D Printer", new Dictionary<string, string> { {"Price", "400"}, {"Power", "-20"}, {"Cooling", "-10"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "4"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "4"}, {"Morale", "1"}, {"SystemRiskMod", "-2"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "-3"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "-10%"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"NASA Data Server", new Dictionary<string, string> { {"Price", "900"}, {"Power", "-35"}, {"Cooling", "-30"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "2"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "-50%"} } },
        {"Radiation Shielding", new Dictionary<string, string> { {"Price", "1500"}, {"Power", "0"}, {"Cooling", "0"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "0"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "-4"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "0"}, {"Radiation Storm (6%)", "-60%"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } },
        {"Fire Suppression", new Dictionary<string, string> { {"Price", "250"}, {"Power", "-5"}, {"Cooling", "-2"}, {"Calories", "0"}, {"Protein", "0"}, {"Carbs", "0"}, {"Fat", "0"}, {"Water", "0"}, {"Oxygen", "0"}, {"Waste", "0"}, {"Storage", "0"}, {"Treatments", "0"}, {"SleepSlots", "0"}, {"ExerciseSlots", "0"}, {"Research", "0"}, {"Morale", "0"}, {"SystemRiskMod", "5"}, {"CommsRiskMod", "0"}, {"NavRiskMod", "0"}, {"EnvRiskMod", "0"}, {"CrewRiskMod", "0"}, {"Heart Attack (5%)", "0"}, {"Illness (8%)", "0"}, {"Malnutrition (forced)", "0"}, {"System Failure (10%)", "0"}, {"Power Loss (7%)", "0"}, {"Fire Hazard (4%)", "-80%"}, {"Radiation Storm (6%)", "0"}, {"Contamination (10%)", "0"}, {"Overheat Spike (8%)", "0"}, {"Collision (10%)", "0"}, {"Data Loss (12%)", "0"} } }
    };

    public string[] listOfElements;

    public string response;

    public async void AskApi(string newText)
    {
        // Convert the dictionary to CSV format
        string csvData = ConvertDictionaryToCsv(objects);

        // Combine the user's text with the CSV data
        string fullPrompt = newText + "\n\nthis data is what the values of the attributes gets changed by for a one month for each object\n" + csvData;
        string survivalGuide = @"Space Habitat Survival Guide (Condensed)

Mission: 3 crew, 3 months

Survival Checks (monthly, per crew)

Fail any → crew death. Multiply by mission length.

Calories 2,500 | Protein 75 | Carbs 300 | Fat 70

Water 30 | Oxygen 30 | Waste 30

Sleep Slots: 1 (fixed) | Exercise Slots: 1 (fixed)

Power, Cooling: balance ≥ 0

Random Events (100% occur for now)

Chance = base ± RiskMods (±3%/step) + object modifiers.

Medical

Heart Attack: 5%/crew | Mods: Defib −80%, Med Packs −10–20%

Illness: 8%/crew | Mods: Med Soft −15%, Amb Pack −25%, Ultrasound −10–15%

Malnutrition: forced if thresholds unmet

System

Failure: 10% | Mods: AI Core −30%, Workstation −15%, Data Hub −10%

Power Loss: 7% | Mods: Battery −15%, Fuel −20%, Reactor −30%

Fire: 4% per fire-prone obj | Mod: Suppression −80%

Environmental

Radiation: 6% | Shield −60%

Contamination: 10% | Mods: Toilet −20%, Recycler −40%, Bioreactor −60%

Overheat: 8% | Mods: Cryo −20%, Exchanger −20%, Radiator −40%

Operational

Collision: 10% | Mods: Nav −25%, AI Core −20%

Data Loss: 12% | Mods: UHF −5%, S −7%, Ku −10%, High-G −15%, Hub −40%, Server −30%

RiskMods

System → Failure, Power, Overheat, Fire

Comms → Data Loss, Nav errors

Nav → Collisions, Debris

Env → Radiation, Contamination, Overheat

Crew → Heart, Illness, Morale
Impact: Prevent = +20 pts | Cause = −20 pts | Neutral if avoided naturally

Treatments

1 unit = 1 save against medical event.

Scoring

Survival: +500/crew alive | −500/death
Research: +20/unit/month | Data Loss = −30%
Morale: Avg > +5 = +10% | < 0 = −10%
Reliability: 0 failures = +10% | >3 = −10%
Budget: ≤2k=+500, 2–4k=+400, 4–6k=+300, 6–8k=+200, 8–10k=+100, >10k=0
Room Size: smaller = more points (1×1=+300 → 20×20=0, descending scale)

Event Results

Heart Attack: −200 death | +100 prevented

Illness: −150 untreated | +80 prevented

Malnutrition: −300 unmet | +150 balanced

Failure: −100 | +60 prevented

Power Loss: −150 | +80 prevented

Fire: −200 | +120 prevented

Radiation: −200 | +120 shielded

Contamination: −150 | +80 prevented

Overheat: −200 | +120 prevented

Collision: −300 | +150 prevented

Data Loss: −200 | +120 prevented

Goal

Survive + optimize. Smaller rooms + lower cost = higher score.

Blocked = bonus

Unmet = penalty

Never happens + unprepared = neutral";

        fullPrompt += "\n" + survivalGuide;


    
        string foundInProj = "A\nNow it's time for the elements found in the ship. Currently, the ship includes:\n";

        foreach (string element in listOfElements)
        {
            foundInProj += element + "\n";
        }
        fullPrompt += foundInProj;
        string[] roomTags = { "Comms", "Control", "Living", "Kitchen", "Medical", "Power", "Thermal", "Waste", "Water-Air", "Storage" };

        StringBuilder roomInfo = new StringBuilder();

        foreach (string tag in roomTags)
        {
            GameObject[] rooms = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject room in rooms)
            {
                roomInfo.AppendLine($"{room.name}");
            }
        }
        fullPrompt += "\nRooms used:\n" + roomInfo.ToString() + "\n";


        Debug.Log(foundInProj.Length);

        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = fullPrompt;
        newMessage.Role = "user";
        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-5-nano"; // Fixed the model name

        var responseData = await openAI.CreateChatCompletion(request);
        response = responseData.Choices[0].Message.Content;

        theContent.SetActive(true);
        Debug.Log(response);

        // Notify the UI to update
        ApiResultUI resultUI = FindObjectOfType<ApiResultUI>();
        if (resultUI != null)
        {
            resultUI.UpdateUIFromAPI(response);
        }
        // REMOVE THIS LINE - it causes errors:
        // Debug.Log(response.Choices[0].Message.Content);
        Debug.Log(fullPrompt);
    }

    private string ConvertDictionaryToCsv(Dictionary<string, Dictionary<string, string>> data)
    {
        if (data.Count == 0) return "";

        StringBuilder csv = new StringBuilder();

        // Get all possible keys/columns from the first item
        var allKeys = new HashSet<string>();
        foreach (var item in data.Values)
        {
            foreach (string key in item.Keys)
            {
                allKeys.Add(key);
            }
        }

        // Create header row
        List<string> headers = new List<string>(allKeys);
        headers.Insert(0, "ItemName"); // Add ItemName as first column

        csv.AppendLine(string.Join(",", headers));

        // Create data rows
        foreach (var item in data)
        {
            string itemName = item.Key;
            List<string> row = new List<string> { $"\"{itemName}\"" }; // Quote the item name

            foreach (string header in headers)
            {
                if (header == "ItemName") continue; // Skip the ItemName column we already added

                if (item.Value.ContainsKey(header))
                {
                    // Quote the value to handle commas in the data
                    row.Add($"\"{item.Value[header]}\"");
                }
                else
                {
                    row.Add("\"0\""); // Default value if key doesn't exist
                }
            }

            csv.AppendLine(string.Join(",", row));
        }

        return csv.ToString();
    }

    // Example method to test sending the data
    public void SendEquipmentData()
    {
        string question = "You are an ai intergrated in a game that firstly uses rooms to layout a spaceship and after the rooms we can add objects to each room\n the space ship is then getting evaluated according to the guides below\n the random events i want you to decide them randomly and build your things around it\n i want you to give me the output in the following format: '---scoreAsAnInteger---', events that happend, how it was saved or should have been saved, '***Tips***' some tip for the next round. ***YourChoices*** 'Your layout was able to: ' and give the carbs fats protiens and things like that. lastly 'How was your score got decided:'";
        AskApi(question);
    }
}