using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Audial{

	public enum FilterState {Bypass, LowPass, LowShelf, HighPass, HighShelf, BandPass, BandAdd}

	[ExecuteInEditMode]
	public class StateVariableFilter : MonoBehaviour {

		private float sampleFrequency;
		void Awake(){
			sampleFrequency = AudioSettings.outputSampleRate;
			UpdateFrequency();
			UpdateDamp();
		}

		private int passes = 2;

		[SerializeField]
		[Range(50, 12000)]
		private float _frequency = 440;
		private double freq;
		public float Frequency{
			get{
				return _frequency;
			}
			set{
				_frequency = Mathf.Clamp(value, 50, 12000);
				UpdateFrequency();
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _resonance = 0.5f;
		public float Resonance{
			get{
				return _resonance;
			}
			set{
				_resonance = Mathf.Clamp(value, 0, 1);
				UpdateDamp();
			}
		}

		[SerializeField]
		[Range(0,0.1f)]
		private float _drive = 0.1f;
		public float Drive{
			get{
				return (0.1f-_drive)+0.001f;
			}
			set{
				_drive = Mathf.Clamp(value, 0, 0.1f);
			}
		}

		public FilterState Filter = FilterState.BandPass;

		[SerializeField]
		[Range(-1,1)]
		private float _additiveGain = 0.25f;
		public float AdditiveGain{
			get{
				return _additiveGain;
			}
			set{
				_additiveGain = Mathf.Clamp(value, -1, 1);
			}
		}

		double[] notch = new double[]{0,0};
		double[] low = new double[]{0,0};
		double[] high = new double[]{0,0};
		double[] band = new double[]{0,0};
		double[] output = new double[]{0,0};

		double damp;

		void UpdateFrequency(){
			freq = 2*Math.Sin(Math.PI*Frequency/(sampleFrequency*passes));
			UpdateDamp();
		}

		void UpdateDamp(){
			damp = Math.Min(2*(1 - Math.Pow(Resonance, 0.25)), Math.Min(2.0 - freq, 2.0/freq - freq*0.5));
		}

		#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;
		private float FrequencyPrev = 0;
		private float ResonancePrev = 0;

		void SetRunEffectInEditMode(bool val){
			runEffectInEditMode = val;
			runEffect = val;
		}
		
		void Update(){
			if(!runEffectInEditMode&&!Application.isPlaying){
				runEffect = false;
				return;
			}
			runEffect = true;
			if(FrequencyPrev!=_frequency||ResonancePrev!=_resonance){
				FrequencyPrev = _frequency;
				ResonancePrev = _resonance;
				UpdateFrequency();
			}
		}
		#endif

		void OnAudioFilterRead(float[] data, int channels){
			#if UNITY_EDITOR
			if(!runEffect) return;
			#endif
			if(Filter == FilterState.Bypass){
				return;
			}

			double[] input = new double[channels];
			for (var i = 0; i < data.Length; i = i + channels){

				for(var c = 0; c < channels; c++){
					input[c] = Math.Abs(data[i+c]) > 10e-8 ? data[i+c] : 0;
					output[c] = 0;


					for(var x = 0; x < passes; x++){
						high[c] = input[c] - low[c] - damp * band[c];
						band[c] = freq * high[c] + band[c] - Drive * Math.Pow(band[c],3);
						low[c] = freq * band[c] + low[c];
					}

					switch(Filter){
					case FilterState.LowShelf:
					case FilterState.LowPass:
						output[c] = low[c];
						break;
					case FilterState.HighShelf:
					case FilterState.HighPass:
						output[c] = high[c];
						break;
					case FilterState.BandAdd:
					case FilterState.BandPass:
						output[c] = band[c];
						break;
					}
					
					if(Filter == FilterState.HighShelf||Filter == FilterState.LowShelf||Filter == FilterState.BandAdd){
						data[i+c] += (float)output[c]*AdditiveGain;
					}else{
						data[i+c] = (float)output[c];
					}

				}
			}
		}
	}
}