using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void Save (GameObject player)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/save.eole";
		FileStream stream = new FileStream(path, FileMode.Create);

		LevelData data = new LevelData(player);

		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static LevelData LoadSave ()
	{
		string path = Application.persistentDataPath + "/save.eole";

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			LevelData data = formatter.Deserialize(stream) as LevelData;
			stream.Close();

			return data;
		}
		else
		{
			return null;
		}
	}
}
