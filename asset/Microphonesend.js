#pragma strict


var a1:AudioClip;

// Start recording with built-in Microphone and play the recorded audio right away
function Rec() {
    audio.clip = Microphone.Start("Built-in Microphone", false, 1, 3000);
    // (deviceName : String, loop : boolean, lengthSec : int, frequency : int) : AudioClip 
    audio.Play();
    print(Microphone.GetPosition("Built-in Microphone"));

}
//getRecordPosition()
function Ply(){
if(a1==null)a1=audio.clip;
	audio.Stop();
    audio.Play();    
    
}
function Ply2(){
audio.Stop();
audio.clip=a1;	
    audio.Play();    
    
}

var targetgo:GameObject;
function dataAudio(){
    // Read all the samples from the clip and half the gain

        var samples:float[] = new float[audio.clip.samples * audio.clip.channels];
        
        audio.clip.GetData(samples, 0);
        targetgo.SendMessage("initbyteImgS",samples);
        
//        audio.clip.SetData(samples, 0);
    
}

function AmplifierAudio(){
    // Read all the samples from the clip and half the gain
if(audio.clip==null)print("mic wait");
else {
        var samples:float[] = new float[audio.clip.samples * audio.clip.channels];
        
        audio.clip.GetData(samples, 0);
        for (var i = 0; i < samples.Length; ++i)
            {
            samples[i] = samples[i] * 1.5f;
            
           }
        
        audio.clip.SetData(samples, 0);
        }
    
}
function OnGUI () {
//print(Microphone.GetPosition("Built-in Microphone"));
if (GUI.Button(Rect(0,0,100,50),"rec"))Rec();
if (GUI.Button(Rect(0,60,100,50),"Play"))Ply();
if (GUI.Button(Rect(0,120,100,50),"Play2"))Ply2();
if (GUI.Button(Rect(0,180,100,50),"AmplifierAudio"))AmplifierAudio();
if (GUI.Button(Rect(0,240,100,50),"dataAudio"))dataAudio();
}
function Start(){
//audio.clip.channels=1;
screenWidth=Screen.width*0.5;
}

function Update(){
SpectrumData1();

}

var screenWidth:float;
function SpectrumData1 () {
    var spectrum : float[] = audio.GetSpectrumData (1024, 0, FFTWindow.BlackmanHarris);
    
    for (var i = 1; i < 1023; i++) {
        Debug.DrawLine (new Vector3 (i - 1 , spectrum[i] + 10, 0), 
                    new Vector3 (i, spectrum[i + 1] + 10, 0), Color.red);
        Debug.DrawLine (new Vector3 (i - 1 , Mathf.Log(spectrum[i - 1]) + 10, 2), 
                    new Vector3 (i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
    
        Debug.DrawLine (new Vector3 (Mathf.Log(i - 1) , spectrum[i - 1] - 10, 1), 
                    new Vector3 (Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
        Debug.DrawLine (new Vector3 (Mathf.Log(i - 1) , Mathf.Log(spectrum[i - 1]), 3), 
                    new Vector3 (Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
    }
}

function SpectrumData2 () {
    var spectrum : float[] = audio.GetSpectrumData (1024, 0, FFTWindow.BlackmanHarris);
    
    for (var i = 1; i < 1023; i++) {
        Debug.DrawLine (new Vector3 (i - 1 -screenWidth, spectrum[i] + 10, 0), 
                    new Vector3 (i, spectrum[i + 1] + 10, 0), Color.red);
        Debug.DrawLine (new Vector3 (i - 1 -screenWidth, Mathf.Log(spectrum[i - 1]) + 10, 2), 
                    new Vector3 (i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
    
        Debug.DrawLine (new Vector3 (Mathf.Log(i - 1) -screenWidth , spectrum[i - 1] - 10, 1), 
                    new Vector3 (Mathf.Log(i) , spectrum[i] - 10, 1), Color.green);
        Debug.DrawLine (new Vector3 (Mathf.Log(i - 1) -screenWidth , Mathf.Log(spectrum[i - 1]), 3), 
                    new Vector3 (Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
    }
}


@script RequireComponent (AudioSource)
//@MenuItem("XKhiariMarwen/MicrophoneScript")
//Created with Khiari Marwen maroien@live.fr
@script AddComponentMenu ("XKhiariMarwen/MicrophoneScript")

/*

AudioImporterLoadType.StreamFromDisc
AudioImporterLoadType.CompressedInMemory
static function GetOutputData (samples : float[], channel : int) : void//Returns a block of the listener (master)'s output data
function GetOutputData (samples : float[], channel : int) : void//Returns a block of the currently playing source's output data
 function Play (delay : UInt64 = 0) : void// audio.Play(44100);  // AudioClip with samplerate of, say, 32 khz, with 16k samples(.5 sec) is done by Play(22050). ((44100/32000) * 16000 = 22050).   
		AudioType.WAV

*/
/*
audio.Play();
// Wait for the audio to have finished
yield WaitForSeconds (audio.clip.length);
*/
/*

function Start () {
    var www : WWW = new WWW("www.example.com");
    audio.clip = www.audioClip;
}

function Update () {
    if(!audio.isPlaying && audio.clip.isReadyToPlay)
        audio.Play();
}    

*/


/*
// Creates a 1 sec long audioclip, with a 440hz sinoid
var position: int = 0;
var sampleRate : int = 0;
var frequency : float = 440;
function Start () {

//delegate PCMReaderCallback (data : float[]) : void
//delegate PCMSetPositionCallback (position : int) : void
//static function Create (name : String, lengthSamples : int, channels : int, frequency : int, _3D : boolean, stream : boolean, pcmreadercallback : PCMReaderCallback, pcmsetpositioncallback : PCMSetPositionCallback) : AudioClip

    var myClip = AudioClip.Create("MySinoid", 44100, 1, 44100, false, true, OnAudioRead, OnAudioSetPosition);
    sampleRate = AudioSettings.outputSampleRate;

    audio.clip = myClip;
    audio.Play();            
}

function OnAudioRead(data:float[])
{
    for (var count = 0; count < data.Length; count++)
    {
        data[count] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / sampleRate));
        position++;
    }
}

function OnAudioSetPosition(newPosition:int)
{
    position = newPosition;
}*/