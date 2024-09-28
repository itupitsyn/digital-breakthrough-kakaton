"use client";

import { Video } from "@/model/video";
import { beautifyDuration } from "@/utils/number";
import axios from "axios";
import { Card } from "flowbite-react";
import Image from "next/image";
import { FC, useCallback, useState } from "react";
import toast from "react-hot-toast";
import { BiArrowToRight, BiImage } from "react-icons/bi";
import { IoMdHeart, IoMdHeartEmpty } from "react-icons/io";
import { PiArrowFatLinesRightFill, PiArrowFatLinesRightLight } from "react-icons/pi";
import { TbPoo, TbPooFilled } from "react-icons/tb";

interface VideoCardProps {
  video: Video;
}

export const VideoCard: FC<VideoCardProps> = ({ video }) => {
  const [previewError, setPreviewError] = useState(false);
  const [videoState, setVideoState] = useState(video.state);

  const onLike = useCallback(async () => {
    try {
      await axios.post(`/api/videos/${video.video_id}/like`);
      setVideoState("like");
    } catch {
      toast.error("Незивестная ошибка =(", { position: "bottom-center" });
    }
  }, [video.video_id]);

  const onDisike = useCallback(async () => {
    try {
      await axios.post(`/api/videos/${video.video_id}/dislike`);
      setVideoState("dislike");
    } catch {
      toast.error("Незивестная ошибка =(", { position: "bottom-center" });
    }
  }, [video.video_id]);

  const onSkip = useCallback(async () => {
    try {
      await axios.post(`/api/videos/${video.video_id}/skip`);
      setVideoState("skip");
    } catch {
      toast.error("Незивестная ошибка =(", { position: "bottom-center" });
    }
  }, [video.video_id]);

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
        <div className="line-clamp-3 whitespace-pre-wrap text-sm" title={video.description}>
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
              <button type="button" aria-label="like" title="like" onClick={onLike}>
                {video.state === "like" ? <IoMdHeart className="size-6" /> : <IoMdHeartEmpty className="size-6" />}
              </button>
            </div>
            <div>
              <button type="button" aria-label="dislike" title="dislike" onClick={onDisike}>
                {video.state === "dislike" ? <TbPooFilled className="size-6" /> : <TbPoo className="size-6" />}
              </button>
            </div>
            <div>
              <button type="button" aria-label="skip" title="skip" onClick={onSkip}>
                {video.state === "skip" ? (
                  <PiArrowFatLinesRightFill className="size-6" />
                ) : (
                  <PiArrowFatLinesRightLight className="size-6" />
                )}
              </button>
            </div>
            <div className="text-xs">{video.v_likes.toLocaleString("ru-RU", { notation: "compact" })}</div>
            <div className="text-xs">{video.v_dislikes.toLocaleString("ru-RU", { notation: "compact" })}</div>
          </div>
        </div>
      </div>
    </Card>
  );
};
