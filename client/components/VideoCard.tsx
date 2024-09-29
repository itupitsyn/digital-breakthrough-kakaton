"use client";

import { Video } from "@/model/video";
import { beautifyDuration } from "@/utils/number";
import axios from "axios";
import { Card } from "flowbite-react";
import Image from "next/image";
import { FC, useCallback, useState } from "react";
import toast from "react-hot-toast";
import { BiImage } from "react-icons/bi";
import { IoMdHeart, IoMdHeartEmpty } from "react-icons/io";
import { PiArrowFatLinesRightFill, PiArrowFatLinesRightLight } from "react-icons/pi";
import { TbPoo, TbPooFilled } from "react-icons/tb";

interface VideoCardProps {
  video: Video;
}

export const VideoCard: FC<VideoCardProps> = ({ video }) => {
  const [previewError, setPreviewError] = useState(false);
  const [videoState, setVideoState] = useState(video.state);
  const [stat, setStat] = useState({ likes: video.v_likes, dislikes: video.v_dislikes });
  const [isLoading, setIsLoading] = useState(false);

  const onLike = useCallback(async () => {
    try {
      setIsLoading(true);
      await axios.post(`/api/videos/${video.video_id}/like`);

      setStat((prev) => {
        if (videoState === "like") return prev;
        else if (videoState === "dislike") return { likes: prev.likes + 1, dislikes: prev.dislikes - 1 };
        return { likes: prev.likes + 1, dislikes: prev.dislikes };
      });
      setVideoState("like");
    } catch {
      toast.error("Незивестная ошибка =(", { position: "bottom-center" });
    } finally {
      setIsLoading(false);
    }
  }, [video.video_id, videoState]);

  const onDisike = useCallback(async () => {
    try {
      setIsLoading(true);
      await axios.post(`/api/videos/${video.video_id}/dislike`);

      setStat((prev) => {
        if (videoState === "dislike") return prev;
        else if (videoState === "like") return { likes: prev.likes - 1, dislikes: prev.dislikes + 1 };
        return { likes: prev.likes, dislikes: prev.dislikes + 1 };
      });
      setVideoState("dislike");
    } catch {
      toast.error("Незивестная ошибка =(", { position: "bottom-center" });
    } finally {
      setIsLoading(false);
    }
  }, [video.video_id, videoState]);

  const onSkip = useCallback(async () => {
    try {
      setIsLoading(true);
      await axios.post(`/api/videos/${video.video_id}/skip`);

      setStat((prev) => {
        if (videoState === "skip" || !videoState) return prev;
        else if (videoState === "like") return { likes: prev.likes - 1, dislikes: prev.dislikes };
        else return { likes: prev.likes, dislikes: prev.dislikes - 1 };
      });
      setVideoState("skip");
    } catch {
      toast.error("Незивестная ошибка =(", { position: "bottom-center" });
    } finally {
      setIsLoading(false);
    }
  }, [video.video_id, videoState]);

  return (
    <Card
      theme={{
        root: {
          children: "flex h-full flex-col justify-between gap-4",
        },
      }}
      className="h-full [&:hover_.pupsik]:scale-110"
    >
      <div className="overflow-hidden rounded-t-lg">
        {previewError || !video.preview ? (
          <div className="pupsik flex h-36 w-full items-center justify-center transition-transform">
            <BiImage className="size-2/3" />
          </div>
        ) : (
          <Image
            src={video.preview}
            width={512}
            height={512}
            alt="video preview"
            className="pupsik h-36 w-full object-cover transition-transform"
            onError={() => setPreviewError(true)}
          />
        )}
      </div>
      <div className="flex grow flex-col justify-between gap-2 px-5 pb-5">
        <h5 className="truncate font-medium tracking-tight" title={video.title}>
          {video.title}
        </h5>
        <div className="line-clamp-3 grow whitespace-pre-wrap text-sm" title={video.description}>
          {video.description}
        </div>
        <div className="flex items-end justify-between gap-4 overflow-hidden">
          <div className="overflow-hidden">
            <div className="text-sm opacity-80">{new Date(video.v_pub_datetime).toLocaleDateString("ru-RU")}</div>
            <div className="truncate text-sm" title={beautifyDuration(video.v_duration)}>
              {beautifyDuration(video.v_duration)}
            </div>
          </div>

          <div className="grid grid-cols-3 place-items-center gap-x-2 text-cyan-500">
            <div>
              <button type="button" aria-label="like" title="like" onClick={onLike} disabled={isLoading}>
                {videoState === "like" ? <IoMdHeart className="size-6" /> : <IoMdHeartEmpty className="size-6" />}
              </button>
            </div>
            <div>
              <button type="button" aria-label="dislike" title="dislike" onClick={onDisike} disabled={isLoading}>
                {videoState === "dislike" ? <TbPooFilled className="size-6" /> : <TbPoo className="size-6" />}
              </button>
            </div>
            <div>
              <button type="button" aria-label="skip" title="skip" onClick={onSkip} disabled={isLoading}>
                {videoState === "skip" ? (
                  <PiArrowFatLinesRightFill className="size-6" />
                ) : (
                  <PiArrowFatLinesRightLight className="size-6" />
                )}
              </button>
            </div>
            <div className="text-xs">{stat.likes.toLocaleString("ru-RU", { notation: "compact" })}</div>
            <div className="text-xs">{stat.dislikes.toLocaleString("ru-RU", { notation: "compact" })}</div>
          </div>
        </div>
      </div>
    </Card>
  );
};
