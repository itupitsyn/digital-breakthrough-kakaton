import { Recommended } from "@/components/Recommended";
import { VideoCard } from "@/components/VideoCard";
import { Video } from "@/model/video";
import axios from "axios";

export default async function Home() {
  const [videosResponse, recommendedResponse] = await Promise.all([
    axios.get<Video[]>(`${process.env.API_URL}/videos`),
    axios.get<Video[]>(`${process.env.API_URL}/videos/recommended`),
  ]);

  return (
    <main className="mt-10 flex flex-col items-stretch gap-6">
      <Recommended videos={recommendedResponse.data} />
      <hr className="border-cyan-500" />
      <div className="grid grid-cols-[repeat(auto-fit,minmax(300px,1fr))] gap-4">
        {videosResponse.data.map((item) => (
          <VideoCard video={item} key={item.id} />
        ))}
      </div>
    </main>
  );
}
