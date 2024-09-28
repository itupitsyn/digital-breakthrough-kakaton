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
    <main className="mt-10 flex flex-col items-stretch gap-10">
      <Recommended videos={recommendedResponse.data} />
      <hr className="border-cyan-500" />
      <div className="grid grid-cols-[repeat(auto-fit,minmax(300px,1fr))] gap-x-4 gap-y-6">
        {videosResponse.data.map((item) => (
          <VideoCard video={item} key={item.id} />
        ))}
      </div>
    </main>
  );
}
