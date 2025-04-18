from pydub import AudioSegment
import itertools
import os

# Rutas donde están tus archivos
bass_tracks = {
    'A': 'BassA.wav',
    'B': 'BassB.wav',
    'C': 'BassC.wav',
    'D': 'BassD.wav'
}

chords_tracks = {
    'A': 'ChordsA.wav',
    'B': 'ChordsB.wav',
    'C': 'ChordsC.wav',
    'D': 'ChordsD.wav'
}

melody_tracks = {
    'A': 'MelodyA.wav',
    'B': 'MelodyB.wav',
    'C': 'MelodyC.wav',
    'D': 'MelodyD.wav'
}

# Una sola percusión
percussion_track = AudioSegment.from_file('Percussion.wav')

# Crear carpeta de salida
output_folder = 'mixed_tracks'
os.makedirs(output_folder, exist_ok=True)

# Generar combinaciones (Bass, Chords, Melody)
for bass_key, chords_key, melody_key in itertools.product(bass_tracks.keys(), chords_tracks.keys(), melody_tracks.keys()):
    bass = AudioSegment.from_file(bass_tracks[bass_key])
    chords = AudioSegment.from_file(chords_tracks[chords_key])
    melody = AudioSegment.from_file(melody_tracks[melody_key])

    # Asegurarse que todos tienen la misma duración (opcional: puedes ajustar esto como quieras)
    min_len = min(len(bass), len(chords), len(melody), len(percussion_track))
    bass = bass[:min_len]
    chords = chords[:min_len]
    melody = melody[:min_len]
    percussion = percussion_track[:min_len]

    # Mezclar
    mix = bass.overlay(chords).overlay(melody).overlay(percussion)

    # Nombre de la mezcla
    filename = f"{bass_key}{chords_key}{melody_key}.wav"
    output_path = os.path.join(output_folder, filename)

    # Exportar
    mix.export(output_path, format="wav")

    print(f"Exported {filename}")

print("Todas las mezclas se han exportado.")
