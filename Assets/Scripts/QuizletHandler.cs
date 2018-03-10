using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuizletHandler : MonoBehaviour {

	private const string API_KEY = "Hd5x4wJFkAuDBMXb2YgyzC";
	private const string CLIENT_ID = "6KTW6QMZ6M";
	private string currentSetID;
	private string currentURL;
	public GameObject loadingIcon;
	public TextMeshPro testText;
	public static QuizletHandler instance = null;
	string initialFlashcardSet = "{\"id\":172072325,\"url\":\"https://quizlet.com/172072325/states-and-capitals-statecapital-flash-cards/\",\"title\":\"States and Capitals (State/Capital)\",\"created_by\":\"Emily_Hayden6\",\"term_count\":50,\"created_date\":1480617037,\"modified_date\":1480617057,\"published_date\":1480617057,\"has_images\":true,\"subjects\":[],\"visibility\":\"public\",\"editable\":\"only_me\",\"has_access\":true,\"can_edit\":false,\"description\":\"\",\"lang_terms\":\"en\",\"lang_definitions\":\"en\",\"password_use\":0,\"password_edit\":0,\"access_type\":2,\"creator_id\":33326684,\"creator\":{\"username\":\"Emily_Hayden6\",\"account_type\":\"free\",\"profile_image\":\"https://up.quizlet.com/jub18-xe52q-256s.jpg\",\"id\":33326684},\"class_ids\":[68660,708498,780056,936395,1302017,1785409,1811090,1811091,1811094,1811098,1849649,1861378,1947044,2111889,2164217,2260144,2514670,2649831,2664774,2791638,3078639,3078643,3080974,3082141,3104285,3110836,3169125,3196653,3197203,3231693,3299019,3299557,3353105,3380929,3443325,3443376,3502048,3507526,3544325,3548006,3551770,3619818,3619836,3693861,3770720,3770843,3770844,3808929,3879470,3919631,3935765,3936216,3967875,3968527,3988741,4002953,4072651,4092537,4113764,4126261,4143371,4147702,4203452,4224894,4252781,4256762,4269634,4284902,4291680,4307849,4309367,4317018,4318961,4322730,4346857,4354068,4356910,4363627,4399369,4399383,4401793,4402449,4402921,4409864,4427585,4438196,4438573,4444437,4445496,4459897,4459907,4480477,4578610,4582753,4605398,4616184,4628526,4634550,4639442,4646771,4651690,4658423,4663493,4668305,4668361,4670210,4732408,4862019,4862020,4877896,4880628,4880739,4884968,4898172,4899316,4925791,4932122,4934131,4958968,4958971,4976496,4980238,4984583,4997556,5007145,5008213,5012179,5024836,5038770,5039288,5041446,5048592,5048961,5060174,5068524,5081268,5084321,5117326,5125681,5129542,5132351,5139393,5145522,5179611,5185025,5188734,5193806,5206236,5217826,5218277,5223210,5224128,5226427,5241541,5247919,5260159,5290226,5296081,5305007,5317177,5341452,5352885,5352889,5360288,5385293,5411453,5418712,5438953,5452766,5469952,5512295,5522914,5542579,5547022,5547142,5547143,5547147,5547149,5547150,5547151,5550056,5550228,5558291,5565010,5573866,5575296,5579764,5602770,5606401,5611120,5618270,5634327,5643416,5652760,5653981,5664736,5668464,5673148,5684096,5721468,5732553,5800200,5802764,5819353,5822690,5826658,5830333,5859857,5917344,5931013,5934763,5938394,5963843,5975446,5979785,5980208,5987807,6000108,6016583,6041404,6053657,6055071,6098495,6101798,6108617,6112865,6162611,6167290,6182862,6199480,6214013,6217000,6219192,6234158,6250469,6261936,6296801,6332236,6333797,6339357,6348301,6433583,6470136,6504707,6559175,6621997,6647764],\"terms\":[{\"id\":5561781084,\"term\":\"Maine\",\"definition\":\"Augusta\",\"image\":{\"url\":\"https://o.quizlet.com/i/pIDAnTNNX1z9ZBqsc8qXkw_m.jpg\",\"width\":240,\"height\":157},\"rank\":0},{\"id\":5561781085,\"term\":\"New Hampshire\",\"definition\":\"Concord\",\"image\":{\"url\":\"https://o.quizlet.com/i/I_nni0CplilQcQ3x0DGvLQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":1},{\"id\":5561781086,\"term\":\"Vermont\",\"definition\":\"Montpelier\",\"image\":{\"url\":\"https://o.quizlet.com/i/WN7SioMWKUhopNu0uXriYg_m.jpg\",\"width\":240,\"height\":157},\"rank\":2},{\"id\":5561781087,\"term\":\"Massachusetts\",\"definition\":\"Boston\",\"image\":{\"url\":\"https://o.quizlet.com/i/WsSF7av91dLsIhLpeq_GQA_m.jpg\",\"width\":240,\"height\":157},\"rank\":3},{\"id\":5561781088,\"term\":\"Rhode Island\",\"definition\":\"Providence\",\"image\":{\"url\":\"https://o.quizlet.com/i/bIuAY2YISLPnBP2vXEivpg_m.jpg\",\"width\":240,\"height\":157},\"rank\":4},{\"id\":5561781089,\"term\":\"Connecticut\",\"definition\":\"Hartford\",\"image\":{\"url\":\"https://o.quizlet.com/i/_Er9cRnc6ram-CGVJB3LTA_m.jpg\",\"width\":240,\"height\":157},\"rank\":5},{\"id\":5561781090,\"term\":\"New York\",\"definition\":\"Albany\",\"image\":{\"url\":\"https://o.quizlet.com/i/6jd5sLyaJLymglmwmdYgFA_m.jpg\",\"width\":240,\"height\":157},\"rank\":6},{\"id\":5561781091,\"term\":\"Pennsylvania\",\"definition\":\"Harrisburg\",\"image\":{\"url\":\"https://o.quizlet.com/i/nbINf-r1QN6Ht6T1-MPh8w_m.jpg\",\"width\":240,\"height\":157},\"rank\":7},{\"id\":5561781092,\"term\":\"New Jersey\",\"definition\":\"Trenton\",\"image\":{\"url\":\"https://o.quizlet.com/i/AK4Y1Plpdx_B4TTltxhRMw_m.jpg\",\"width\":240,\"height\":157},\"rank\":8},{\"id\":5561781093,\"term\":\"Delaware\",\"definition\":\"Dover\",\"image\":{\"url\":\"https://o.quizlet.com/i/Ckki-osiDJw-LD0gnBmzxw_m.jpg\",\"width\":240,\"height\":157},\"rank\":9},{\"id\":5561781094,\"term\":\"Maryland\",\"definition\":\"Annapolis\",\"image\":{\"url\":\"https://o.quizlet.com/i/YkWVz66QrQVHbWz35KfDKw_m.jpg\",\"width\":240,\"height\":157},\"rank\":10},{\"id\":5561781095,\"term\":\"West Virginia\",\"definition\":\"Charleston\",\"image\":{\"url\":\"https://o.quizlet.com/i/Fj97eYKZS5mqrf_zRTs1TA_m.jpg\",\"width\":240,\"height\":157},\"rank\":11},{\"id\":5561781096,\"term\":\"Virginia\",\"definition\":\"Richmond\",\"image\":{\"url\":\"https://o.quizlet.com/i/SyXKlqgPTJyNDygATqXCbw_m.jpg\",\"width\":240,\"height\":157},\"rank\":12},{\"id\":5561781097,\"term\":\"North Carolina\",\"definition\":\"Raleigh\",\"image\":{\"url\":\"https://o.quizlet.com/i/8NwKQtBUP6nOnre0WAJTQg_m.jpg\",\"width\":240,\"height\":157},\"rank\":13},{\"id\":5561781098,\"term\":\"South Carolina\",\"definition\":\"Columbia\",\"image\":{\"url\":\"https://o.quizlet.com/i/-qO837e3zicYCv9Fr1BFbw_m.jpg\",\"width\":240,\"height\":157},\"rank\":14},{\"id\":5561781099,\"term\":\"Georgia\",\"definition\":\"Atlanta\",\"image\":{\"url\":\"https://o.quizlet.com/i/CENn-0zYFl9s3Q8h67Nn_g_m.jpg\",\"width\":240,\"height\":157},\"rank\":15},{\"id\":5561781100,\"term\":\"Florida\",\"definition\":\"Tallahassee\",\"image\":{\"url\":\"https://o.quizlet.com/i/kXWgZQJp5QC5FTMPyMXnXw_m.jpg\",\"width\":240,\"height\":157},\"rank\":16},{\"id\":5561781101,\"term\":\"Alabama\",\"definition\":\"Montgomery\",\"image\":{\"url\":\"https://o.quizlet.com/i/nJmgG8vnuHFOuGAt5xK7-g_m.jpg\",\"width\":240,\"height\":157},\"rank\":17},{\"id\":5561781102,\"term\":\"Mississippi\",\"definition\":\"Jackson\",\"image\":{\"url\":\"https://o.quizlet.com/i/0NwF0scxCAL6GUZMhlCEwQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":18},{\"id\":5561781103,\"term\":\"Louisiana\",\"definition\":\"Baton Rouge\",\"image\":{\"url\":\"https://o.quizlet.com/i/XxAaig-_R9Jnpez87HPnJQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":19},{\"id\":5561781104,\"term\":\"Arkansas\",\"definition\":\"Little Rock\",\"image\":{\"url\":\"https://o.quizlet.com/i/EAOcFdnNu9cMd3ZW7LchxA_m.jpg\",\"width\":240,\"height\":157},\"rank\":20},{\"id\":5561781105,\"term\":\"Tennessee\",\"definition\":\"Nashville\",\"image\":{\"url\":\"https://o.quizlet.com/i/mcG2OOGdEyXYHJxcMtUXsA_m.jpg\",\"width\":240,\"height\":157},\"rank\":21},{\"id\":5561781106,\"term\":\"Kentucky\",\"definition\":\"Frankfort\",\"image\":{\"url\":\"https://o.quizlet.com/i/WG6kXZ5iuQ98HPLVhbcgWA_m.jpg\",\"width\":240,\"height\":157},\"rank\":22},{\"id\":5561781107,\"term\":\"Ohio\",\"definition\":\"Columbus\",\"image\":{\"url\":\"https://o.quizlet.com/i/2r8HP9J4lrsmR2lZGozkgg_m.jpg\",\"width\":240,\"height\":157},\"rank\":23},{\"id\":5561781108,\"term\":\"Indiana\",\"definition\":\"Indianapolis\",\"image\":{\"url\":\"https://o.quizlet.com/i/OMFuponxo83_xajOFWSCFw_m.jpg\",\"width\":240,\"height\":157},\"rank\":24},{\"id\":5561781109,\"term\":\"Michigan\",\"definition\":\"Lansing\",\"image\":{\"url\":\"https://o.quizlet.com/i/39rFUVENBvbL_9D_4Kj0jw_m.jpg\",\"width\":240,\"height\":157},\"rank\":25},{\"id\":5561781110,\"term\":\"Illinois\",\"definition\":\"Springfield\",\"image\":{\"url\":\"https://o.quizlet.com/i/sGoBo8MEQOT88dxlcvW4HQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":26},{\"id\":5561781111,\"term\":\"Wisconsin\",\"definition\":\"Madison\",\"image\":{\"url\":\"https://o.quizlet.com/i/_YYml7HQOXtGnpcQAs45tw_m.jpg\",\"width\":240,\"height\":157},\"rank\":27},{\"id\":5561781112,\"term\":\"Minnesota\",\"definition\":\"St. Paul\",\"image\":{\"url\":\"https://o.quizlet.com/i/wAROkrUWayiY7hRYIXIsnA_m.jpg\",\"width\":240,\"height\":157},\"rank\":28},{\"id\":5561781113,\"term\":\"Iowa\",\"definition\":\"Des Moines\",\"image\":{\"url\":\"https://o.quizlet.com/i/rDhoDgcept-zGQf5fb1qwA_m.jpg\",\"width\":240,\"height\":157},\"rank\":29},{\"id\":5561781114,\"term\":\"Missouri\",\"definition\":\"Jefferson City\",\"image\":{\"url\":\"https://o.quizlet.com/i/oCdA-Mfj06pkqIpyr0RIEA_m.jpg\",\"width\":240,\"height\":157},\"rank\":30},{\"id\":5561781115,\"term\":\"Texas\",\"definition\":\"Austin\",\"image\":{\"url\":\"https://o.quizlet.com/i/fnfik-iKpm2pxQASwdaLdQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":31},{\"id\":5561781116,\"term\":\"Oklahoma\",\"definition\":\"Oklahoma City\",\"image\":{\"url\":\"https://o.quizlet.com/i/SoXGnWh19w-IDs67uqkpww_m.jpg\",\"width\":240,\"height\":157},\"rank\":32},{\"id\":5561781117,\"term\":\"Kansas\",\"definition\":\"Topeka\",\"image\":{\"url\":\"https://o.quizlet.com/i/Cbn6Fpk3FWSPaxhi2BSr0A_m.jpg\",\"width\":240,\"height\":157},\"rank\":33},{\"id\":5561781118,\"term\":\"Nebraska\",\"definition\":\"Lincoln\",\"image\":{\"url\":\"https://o.quizlet.com/i/rZqj6zyrz0Yc38qhbtiDmw_m.jpg\",\"width\":240,\"height\":157},\"rank\":34},{\"id\":5561781119,\"term\":\"South Dakota\",\"definition\":\"Pierre\",\"image\":{\"url\":\"https://o.quizlet.com/i/Y2KY6lrTj-98FIZQDEOtwQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":35},{\"id\":5561781120,\"term\":\"North Dakota\",\"definition\":\"Bismarck\",\"image\":{\"url\":\"https://o.quizlet.com/i/M0kGFl_fU6Ojo8cRGsDmfg_m.jpg\",\"width\":240,\"height\":157},\"rank\":36},{\"id\":5561781121,\"term\":\"Montana\",\"definition\":\"Helena\",\"image\":{\"url\":\"https://o.quizlet.com/i/ihleHG14TrqKWQt2jhaFGw_m.jpg\",\"width\":240,\"height\":157},\"rank\":37},{\"id\":5561781122,\"term\":\"Wyoming\",\"definition\":\"Cheyenne\",\"image\":{\"url\":\"https://o.quizlet.com/i/v10D6EFLwnCAEqBnr02XaA_m.jpg\",\"width\":240,\"height\":157},\"rank\":38},{\"id\":5561781123,\"term\":\"Colorado\",\"definition\":\"Denver\",\"image\":{\"url\":\"https://o.quizlet.com/i/-xuvXcDsYnapwb1HIpaS8A_m.jpg\",\"width\":240,\"height\":157},\"rank\":39},{\"id\":5561781124,\"term\":\"New Mexico\",\"definition\":\"Santa Fe\",\"image\":{\"url\":\"https://o.quizlet.com/i/47iFby3iJEs8Lvhd9DI7_A_m.jpg\",\"width\":240,\"height\":157},\"rank\":40},{\"id\":5561781125,\"term\":\"Arizona\",\"definition\":\"Phoenix\",\"image\":{\"url\":\"https://o.quizlet.com/i/mK6k4Ts_gY0szqJTDkhaTQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":41},{\"id\":5561781126,\"term\":\"Utah\",\"definition\":\"Salt Lake City\",\"image\":{\"url\":\"https://o.quizlet.com/i/f912oTF7jpPtoyi5Ps0Cpw_m.jpg\",\"width\":240,\"height\":157},\"rank\":42},{\"id\":5561781127,\"term\":\"Idaho\",\"definition\":\"Boise\",\"image\":{\"url\":\"https://o.quizlet.com/i/PGrj2sE4bryU5WUGsmWceg_m.jpg\",\"width\":240,\"height\":157},\"rank\":43},{\"id\":5561781128,\"term\":\"Washington\",\"definition\":\"Olympia\",\"image\":{\"url\":\"https://o.quizlet.com/i/gIIuKZtntaW6ZX-nu-lUJQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":44},{\"id\":5561781129,\"term\":\"Oregon\",\"definition\":\"Salem\",\"image\":{\"url\":\"https://o.quizlet.com/i/gU9CbOvonp8O3KyBofIm1Q_m.jpg\",\"width\":240,\"height\":157},\"rank\":45},{\"id\":5561781130,\"term\":\"Nevada\",\"definition\":\"Carson City\",\"image\":{\"url\":\"https://o.quizlet.com/i/vbF-emlR1lxAhW7UOJqDrA_m.jpg\",\"width\":240,\"height\":157},\"rank\":46},{\"id\":5561781131,\"term\":\"California\",\"definition\":\"Sacramento\",\"image\":{\"url\":\"https://o.quizlet.com/i/jOOlLQgBBa7Gi8HuXao5Bw_m.jpg\",\"width\":240,\"height\":157},\"rank\":47},{\"id\":5561781132,\"term\":\"Hawaii\",\"definition\":\"Honolulu\",\"image\":{\"url\":\"https://o.quizlet.com/i/Lw0aiQ-dWN0xPFHyNZpiZQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":48},{\"id\":5561781133,\"term\":\"Alaska\",\"definition\":\"Juneau\",\"image\":{\"url\":\"https://o.quizlet.com/i/kJ-lAnrNtScFbtU9pHGiLQ_m.jpg\",\"width\":240,\"height\":157},\"rank\":49}]}";


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		//Request ();
		ImportInitialFlashcardSet();
	}
	void ImportInitialFlashcardSet(){
		FlashcardSet f = JsonUtility.FromJson<FlashcardSet> (initialFlashcardSet);
		f.jsonString = initialFlashcardSet;
		FlashcardManager.instance.AddFlashcardSet (f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void UpdateSetID(Text id){
		currentSetID = id.text;
		UpdateURL ();
	}
	private void UpdateURL(){
		currentURL = "https://api.quizlet.com/2.0/sets/" + currentSetID + "?client_id=" + CLIENT_ID;
		Request (currentURL);
	}
	private const string testURL = "https://api.quizlet.com/2.0/sets/125969193?client_id=6KTW6QMZ6M";

	public void Request(string reqURL){
		loadingIcon.SetActive (true);
		WWW request = new WWW (reqURL);
		StartCoroutine (OnResponse(request));
	}
	IEnumerator OnResponse(WWW req){
		yield return req;
		print ("done");
		loadingIcon.SetActive (false);
		FlashcardSet f = JsonUtility.FromJson<FlashcardSet> (req.text);
		f.jsonString = req.text;
		//TODO: make on screen indication
		if (f.http_code == 404)
			print ("Invalid set ID");
		else 
			FlashcardManager.instance.AddFlashcardSet (f);

		//testText.text = f.ToString();
		//testText.text = req.text;
	}
}
/*
		Application: 
		FlashcardTD

		Your Quizlet Client ID (used for public and user access):
		6KTW6QMZ6M

		Your Secret Key (used for user authentication only):
		Hd5x4wJFkAuDBMXb2YgyzC (reset)
		
		Your Redirect URI:
		flashcardtd://after_oauth (double-click to edit) 
*/