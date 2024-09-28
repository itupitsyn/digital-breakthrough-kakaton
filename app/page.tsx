import { Recommended } from "@/components/Recommended";
import { VideoGrid } from "@/components/VideoGrid";
import { Video } from "@/model/video";
import axios from "axios";

export default async function Home() {
  const [videosResponse, recommendedResponse] = await Promise.all([
    axios.get<Video[]>(`${process.env.API_URL}/videos`),
    axios.get<Video[]>(`${process.env.API_URL}/videos/recommended`),
  ]);

  return (
    <main className="mt-10 flex flex-col items-stretch gap-10">
      <Recommended videos={recommendedResponse.data} />
      <hr className="border-cyan-500" />
      <VideoGrid videos={videosResponse.data} />
    </main>
  );
}
