import { VideoCard } from "@/components/VideoCard";
import { Video } from "@/model/video";
import axios from "axios";

export default async function Home() {
  const response = await axios.get<Video[]>("http://localhost:3000/api/videos");

  return (
    <main className="mt-10 flex flex-col items-stretch gap-2">
      <div className="grid grid-cols-[repeat(auto-fit,minmax(300px,1fr))] gap-4">
        {response.data.map((item) => (
          <VideoCard video={item} key={item.id} />
        ))}
      </div>
    </main>
  );
}
