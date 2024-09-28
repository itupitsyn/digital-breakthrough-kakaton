"use client";

import { FC } from "react";
import { VideoCard } from "./VideoCard";
import { Video } from "@/model/video";
import { Spinner } from "./Spinner";
import { useTransitionContext } from "@/contexts/transitionContext";

interface VideoGridProps {
  videos: Video[];
}

export const VideoGrid: FC<VideoGridProps> = ({ videos }) => {
  const [isPending] = useTransitionContext();

  return (
    <div className="relative grid grid-cols-[repeat(auto-fit,minmax(300px,1fr))] gap-x-4 gap-y-6">
      {videos.map((item) => (
        <VideoCard video={item} key={item.video_id} />
      ))}
      {isPending && <Spinner />}
    </div>
  );
};
